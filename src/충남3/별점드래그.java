package 충남3;

import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.GridLayout;
import java.awt.Shape;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.geom.Area;
import java.awt.geom.Rectangle2D;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class 별점드래그 extends JFrame {
	JLabel dis = new JLabel("0.0");
	
	public 별점드래그() {
		ui();
		setVisible(true);
	}
	
	private void ui() {
		setSize(500, 300);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(3);
		add(new MyLabel());
		add(dis, "South");
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(별점드래그::new);
	}
	
	class MyLabel extends JLabel {
		double point;
		
		public MyLabel() {
			addMouseMotionListener(new MouseAdapter() {
				@Override
				public void mouseDragged(MouseEvent e) {
					point = e.getX();
					dis.setText(String.format("%.1f", Math.max(Math.min(point/getWidth()*5,5),0)));
					repaint();
				}
			});
		}
		
		@Override
		protected void paintComponent(Graphics g) {
			super.paintComponent(g);
			Graphics2D g2d = (Graphics2D) g;
			
			Shape txtShp = new Font("맑은 고딕", Font.PLAIN, 100).createGlyphVector(g2d.getFontRenderContext(), "★★★★★").getOutline();
			int x = txtShp.getBounds().x;
			int y = txtShp.getBounds().y;
			int w = txtShp.getBounds().width;
			int h = txtShp.getBounds().height;
			
			g2d.translate(-x, -y);
			
			Area area1 = new Area(new Rectangle2D.Double(x, y, point, h));
			Area area2 = new Area(txtShp);
			area1.intersect(area2);
			
			g2d.setColor(Color.yellow);
			g2d.fill(area1);
			g2d.setColor(Color.black);
			g2d.draw(area2);
		}
	}
}
