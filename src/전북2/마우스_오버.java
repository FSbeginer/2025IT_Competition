package 전북2;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.geom.Ellipse2D;

import javax.swing.JLabel;

public class 마우스_오버 extends JLabel {
	boolean isOver = false;
	
	public 마우스_오버(String txt) {
		setLayout(new BorderLayout());
		add(new JLabel(txt, 0));
		addMouseListener(new MouseAdapter() {
			@Override
			public void mouseEntered(MouseEvent e) {
				isOver = true;
				repaint();
			}
			@Override
			public void mouseExited(MouseEvent e) {
				isOver = false;
				repaint();
			}
		});
		setPreferredSize(new Dimension(0, 50));
	}
	
	@Override
	protected void paintComponent(Graphics g) {
		super.paintComponent(g);
		if(isOver) {
			g.setColor(new Color(170, 200, 230));
			g.fillOval(getWidth()/2-25, 0, 50, 50);
		}
	}
	
	@Override
	public boolean contains(int x, int y) {
		Ellipse2D e2d = new Ellipse2D.Double(getWidth()/2-25, 0, 50, 50);
		return e2d.contains(x, y);
	}
}
