package 전북2;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.GridLayout;
import java.awt.RenderingHints;
import java.awt.geom.AffineTransform;
import java.sql.SQLException;
import java.util.Arrays;

import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 도넛차트_ing extends JFrame {
	
	JComboBox jcb;
	double[] data = new double[5];
	String[] sql = "record,reservation".split(",");
	String[] name = new String[5];
	Color[] c = new Color[] {Color.red, Color.orange, Color.yellow, Color.green, Color.blue};
	JPanel namePanel;
	
	
	public 도넛차트_ing() {
		ui();
		analyzePanel();
		getData(sql[0]);
		setVisible(true);
	}
	
	private void getData(String  sql) {
		try (var rs = Connect.res("select *, count(*) cnt from doctor d join "+sql+" using(dno) group by dno order by cnt desc limit 5;")) {
			for (int i = 0; rs.next(); i++) {
				data[i] = rs.getDouble("cnt");
				name[i] = rs.getString("name");
			}
			double sum = Arrays.stream(data).sum();
			
			for (int i = 0; i < data.length; i++) 
				data[i] = data[i] / sum * 360;
		} catch (SQLException e) {
		}
	}

	private void analyzePanel() {
		JPanel jp = new JPanel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				Graphics2D g2d = (Graphics2D) g;
				g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
				g2d.setFont(new Font("굴림", Font.PLAIN,  12));
				AffineTransform ori = g2d.getTransform();
				
				int ang = 90;
				for (int i = 0; i < 5; i++) {
					g2d.setColor(c[i]); 
					g2d.fillArc(0, 0, 500, 500, ang, -(int)data[i]);
					ang -= data[i];
				}
				
				var fm = g2d.getFontMetrics();
				ang = 90;
				for (int i = 0; i < 5; i++) {
					String percent = String.format("%.1f%%", data[i]/360 * 100);
					g2d.rotate(-Math.toRadians(ang-data[i]/2), 250, 250);
					g2d.rotate(Math.toRadians(ang-data[i]/2), 500-fm.stringWidth(percent)/2, 250);
					
					g2d.setColor(Color.black);
					g2d.drawString(percent, 500-fm.stringWidth(percent)/2, 250);
					
					g2d.setTransform(ori);
					ang -= data[i];
				}
				
				g2d.setColor(Color.white);
				g2d.fillOval(85, 85, 330, 330);
			}
		};
		add(jp);
		
		namePanel = new JPanel();
		namePanel.setPreferredSize(new Dimension(0, 50));
		namePanel.setLayout(new GridLayout(1, 5));
		add(namePanel, "South");
	}

	private void ui() {
		setSize(600, 650);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		jcb = new JComboBox();
		add(jcb, "North");
		jcb.addItem("진료 많은 의사 5");
		jcb.addItem("예약 많은 의사 5");
		jcb.addActionListener((e)->{
			getData(sql[jcb.getSelectedIndex()]);
			repaint();
		});
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 도넛차트_ing();});
	}
}
