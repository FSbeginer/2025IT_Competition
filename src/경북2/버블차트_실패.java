package 경북2;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Rectangle;
import java.awt.Shape;
import java.awt.geom.Area;
import java.awt.geom.Ellipse2D;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 버블차트_실패 extends JFrame{
	
	List<Bubble> items = new ArrayList<버블차트_실패.Bubble>();
	Random r = new Random();
	private JPanel jp;
	
	public 버블차트_실패() {
		ui();
		addPanel();
		getData();
		setVisible(true);
	}
	
	
	private void addPanel() {
		jp = new JPanel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				
				Graphics2D g2d = (Graphics2D)g;
				
				for (Bubble bubble : items) {
					if(bubble.ang<0) bubble.ang = r.nextInt(360);
					
					double x = bubble.x + Math.cos(Math.toRadians(bubble.ang));
					double y = bubble.y + Math.sin(Math.toRadians(bubble.ang));
					
					bubble.setFrame(x, y, bubble.size, bubble.size);
					
					boolean inter = false;
					
					for (Bubble inBubble : items) {
						Area area = new Area(bubble);
						if((inBubble != bubble && area.contains(inBubble.getFrame()))||!new Rectangle(jp.getSize()).contains(bubble.getFrame())) {
							inter = true;
							bubble.ang = -1;
							break;
						}
					}
					
					if(!inter) {
						bubble.x = x;
						bubble.y = y;
					}
					
					g2d.setColor(bubble.c);
					g2d.fill(bubble);
					g2d.setColor(Color.black);
					g2d.draw(bubble);
				}
				
				try {
					Thread.sleep(1);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				
				repaint();
			}
		};
		jp.setSize(getWidth()-20, getHeight()-40);
		add(jp);
	}


	private void getData() {
		try (var rs = Connect.res("select cnam, rank() over(order by count(*) desc)-1, count(*) from product join category c using(cno) join `order` using(pno) group by cno; ")) {
			int maxSize = 0;
			while(rs.next()) {
				maxSize = Math.max(rs.getInt(3)*2, maxSize);
				Color rc = new Color(r.nextInt(256), r.nextInt(256), r.nextInt(256));
				
				while(true) {
					Shape bubble;
					int size = (int) (maxSize * (1-rs.getDouble(2)/10));
					int x = r.nextInt(jp.getWidth()-size);
					int y = r.nextInt(jp.getHeight()-size);
					bubble = new Rectangle(x, y, size, size);
					
					boolean inter = false;
					for (Bubble b: items) {
						if(bubble.intersects(b.getBounds())) {
							inter = true;
							break;
						}
					}
					if(!inter) {
						items.add(new Bubble(rs.getString(1), size, x, y, -1, rc));
						break;
					}
				}
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}


	private void ui() {
		setLocationRelativeTo(null);
		setDefaultCloseOperation(3);
		setSize(600, 400);
	}


	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 버블차트_실패();});
	}
	
	class Bubble extends Ellipse2D.Double {
		String name;
		double size, x, y;
		int ang;
		Color c;
		public Bubble(String name, double size, double x, double y, int ang, Color c) {
			super(x, y, size, size);
			this.name = name;
			this.size = size;
			this.x =  x;
			this.y = y;
			this.ang = ang;
			this.c = c;
		}
	}
}
