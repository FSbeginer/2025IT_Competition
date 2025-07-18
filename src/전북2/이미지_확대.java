package 전북2;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;
import java.awt.image.BufferedImage;

import javax.swing.ImageIcon;
import javax.swing.JLabel;

public class 이미지_확대 extends JLabel {
	private boolean isEnter;
	Point mp, prev;
	double scale = 1.0;
	Image img;

	public 이미지_확대(ImageIcon icon) {
		super(icon);
		img = icon.getImage();

		addMouseMotionListener(new MouseAdapter() {
			@Override
			public void mouseDragged(MouseEvent e) {
				if (prev == null)
					prev = e.getPoint();
				scale = Math.max(scale + (e.getY() - prev.y > 0 ? 0.01 : -0.01), 0);
				prev = e.getPoint();
			}

			@Override
			public void mouseMoved(MouseEvent e) {
				mp = e.getPoint();
			}
		});
		addMouseListener(new MouseAdapter() {
			@Override
			public void mouseEntered(MouseEvent e) {
				isEnter = true;
			}

			@Override
			public void mouseExited(MouseEvent e) {
				isEnter = false;
			}
		});
		addMouseWheelListener(new MouseAdapter() {
			@Override
			public void mouseWheelMoved(MouseWheelEvent e) {
				scale = Math.max(scale + (e.getWheelRotation() > 0 ? 0.1 : -0.1), 0);
			}
		});
	}

	@Override
	protected void paintComponent(Graphics g) {
		super.paintComponent(g);
		if (isEnter) {
			Graphics2D g2d = (Graphics2D) g;
			int x = mp.x;
			int y = mp.y;
			int sx = (int) (x / scale);
			int sy = (int) (y / scale);

			g2d.scale(scale, scale);

			g2d.drawImage(img, sx - 25, sy - 25, sx + 25, sy + 25, x - 25, y - 25, x + 25, y + 25, null);
			g2d.setColor(Color.red);
			g2d.drawRect(sx - 25, sy - 25, 50, 50);
		}
		repaint();
	}
}
