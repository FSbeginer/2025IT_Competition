package 경북2;

import java.awt.Color;
import java.awt.Font;
import java.awt.FontMetrics;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.RenderingHints;
import java.awt.geom.Ellipse2D;
import java.awt.image.BufferedImage;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 버블차트3 extends JFrame {

	List<Bubble> bubbles = new ArrayList<>();
	Random rand = new Random();
	JPanel jp;

	public 버블차트3() {
		ui();
		addPanel();
		// Run data loading in a separate thread to avoid blocking the UI
		new Thread(this::getData).start();
		setVisible(true);
	}

	private void addPanel() {
		jp = new JPanel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				Graphics2D g2d = (Graphics2D) g;
				g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

				// --- Physics and Drawing Loop ---

				// 1. Move all bubbles first
				for (Bubble b : bubbles) {
					b.move();
				}

				// 2. Handle all collisions (bubble-bubble and wall) in a separate step
				// Using multiple passes can improve stability
				for (int i = 0; i < 2; i++) { // Iterate a couple of times for stability
					handleWallCollisions();
					handleBubbleCollisions();
				}


				// 3. Draw all bubbles and text
				for (Bubble b : bubbles) {
					// Draw Bubble
					g2d.setColor(b.c);
					g2d.fill(b);
					g2d.setColor(b.c.darker());
					g2d.draw(b);

					// Draw Text
					g2d.setColor(Color.WHITE);
					Font font = new Font("HY견고딕", Font.BOLD, (int)(b.size / 5)); // Font size scales with bubble
					g2d.setFont(font);
					FontMetrics fm = g2d.getFontMetrics();
					int textWidth = fm.stringWidth(b.name);
					int textHeight = fm.getAscent() - fm.getDescent();
					g2d.drawString(b.name, (float)(b.getCenterX() - textWidth / 2.0), (float)(b.getCenterY() + textHeight / 2.0));
				}

				try {
					Thread.sleep(10); // Adjust for animation speed
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				repaint();
			}

			private void handleWallCollisions() {
				for (Bubble b : bubbles) {
					if (b.x < 0) {
						b.x = 0;
						b.vx *= -1; // Reflect horizontal velocity
					} else if (b.x + b.size > jp.getWidth()) {
						b.x = jp.getWidth() - b.size;
						b.vx *= -1;
					}

					if (b.y < 0) {
						b.y = 0;
						b.vy *= -1; // Reflect vertical velocity
					} else if (b.y + b.size > jp.getHeight()) {
						b.y = jp.getHeight() - b.size;
						b.vy *= -1;
					}
				}
			}

			private void handleBubbleCollisions() {
				for (int i = 0; i < bubbles.size(); i++) {
					for (int j = i + 1; j < bubbles.size(); j++) {
						Bubble b1 = bubbles.get(i);
						Bubble b2 = bubbles.get(j);

						double dx = b2.getCenterX() - b1.getCenterX(); // 두 버블의 x좌표 차이
						double dy = b2.getCenterY() - b1.getCenterY(); // 두 버블의 y좌표 차이
						double distance = Math.hypot(dx, dy); // 중점간 현재거리
						double minDistance = b1.size / 2 + b2.size / 2; // d 중점간 최소거리(겹쳤을때)

						if (distance < minDistance) {
							// --- Step 1: Resolve Overlap ---
							double overlap = (minDistance - distance); // 최소거리 - 현재거리 (겹쳤다면 겹친 거리 나옴)
							double pushX = (dx / distance) * overlap * 0.5; // cos * 
							double pushY = (dy / distance) * overlap * 0.5; // sin * 

							b1.x -= pushX;
							b1.y -= pushY;
							b2.x += pushX;
							b2.y += pushY;
							
							// --- Step 2: Calculate Bounce (Elastic Collision) ---
							double normalX = dx / distance; // cos
							double normalY = dy / distance; // sin

							// dot product of velocity and normal
							double p1 = b1.vx * normalX + b1.vy * normalY;
							double p2 = b2.vx * normalX + b2.vy * normalY;
							
							// new velocities along the normal (mass is proportional to area)
							double m1 = b1.size * b1.size;
							double m2 = b2.size * b2.size;
							// p1 법선 백터에 내적 곱함 * (m1 - m2) 즉 크기에 따른만큼 그 힘을 뿔리거나 줄임
							// p2 또한 마찬가지로 구함 그래서 속도가 빨라지거나 느려짐
							double v1_new = (p1 * (m1 - m2) + 2 * m2 * p2) / (m1 + m2);
							double v2_new = (p2 * (m2 - m1) + 2 * m1 * p1) / (m1 + m2);

							b1.vx += (v1_new - p1) * normalX;
							b1.vy += (v1_new - p1) * normalY;
							b2.vx += (v2_new - p2) * normalX;
							b2.vy += (v2_new - p2) * normalY;
						}
					}
				}
			}
		};
		add(jp);
	}

	private void getData() {
		// Wait until the panel is actually drawn and has a size
		while (jp.getWidth() == 0 || jp.getHeight() == 0) {
			try {
				Thread.sleep(50);
			} catch (InterruptedException e) { e.printStackTrace(); }
		}

		try (var rs = Connect.res("select cnam, rank() over(order by count(*) desc)-1, count(*) from product join category c using(cno) join `order` using(pno) group by cno")) {
			
			double maxCount = 0;
			List<Object[]> tempData = new ArrayList<>();
			while (rs.next()) {
				double count = rs.getDouble(3);
				maxCount = Math.max(count, maxCount);
				tempData.add(new Object[]{rs.getString(1), rs.getDouble(2), count});
			}

			
			//0.0~1.0
			for (Object[] row : tempData) {
				double size = 40 + ( (double)row[2] / maxCount) * 80; // Calculate size based on max count
				Color rc = Color.getHSBColor(rand.nextFloat(), 0.6f, 0.9f);
				bubbles.add(new Bubble((String)row[0], 0, 0, size, rc));
			}

			// Initial placement logic
			for (int i = 0; i < bubbles.size(); i++) {
				Bubble b1 = bubbles.get(i);
				boolean placed = false;
				int tries = 0;
				while (!placed && tries < 1000) {
					b1.x = rand.nextInt(Math.max(1, jp.getWidth() - (int) b1.size));
					b1.y = rand.nextInt(Math.max(1, jp.getHeight() - (int) b1.size));
					
					boolean overlaps = false;
					for (int j = 0; j < i; j++) {
						Bubble b2 = bubbles.get(j);
						double dist = Math.hypot(b1.getCenterX() - b2.getCenterX(), b1.getCenterY() - b2.getCenterY());
						if (dist < (b1.size / 2 + b2.size / 2)) {
							overlaps = true;
							break;
						}
					}
					if (!overlaps) {
						placed = true;
					}
					tries++;
				}
			}

		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	private void ui() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setSize(800, 600);
		setLocationRelativeTo(null);
		setTitle("Bubble Chart Animation");
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(버블차트3::new);
	}

	class Bubble extends Ellipse2D.Double {
		String name;
		double size;
		double vx, vy; // Velocity components
		Color c;

		public Bubble(String name, double x, double y, double size, Color c) {
			super(x, y, size, size);
			this.name = name;
			this.size = size;
			this.c = c;
			
			// Start with a random direction and speed
			double initialAngle = rand.nextDouble() * 2 * Math.PI;
			double initialSpeed = 1.0;
			this.vx = Math.cos(initialAngle) * initialSpeed;
			this.vy = Math.sin(initialAngle) * initialSpeed;
		}

		public void move() {
			this.x += vx;
			this.y += vy;
			this.setFrame(x, y, size, size);
		}

		public double getCenterX() {
			return x + size / 2;
		}

		public double getCenterY() {
			return y + size / 2;
		}
	}
}