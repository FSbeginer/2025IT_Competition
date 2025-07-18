package 전북2;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Image;
import java.awt.Point;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;

import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class 레이더_애니메이션 extends JLabel {
	
	public static void main(String[] args) {
		SwingUtilities.invokeLater(() -> {
			new JFrame() {
				{
					setSize(400, 400);
					setDefaultCloseOperation(3);
					setLocationRelativeTo(null);
					add(new 레이더_애니메이션(new ImageIcon(new ImageIcon("./전북2/map.png").getImage().getScaledInstance(400, 400, 4))));
					setVisible(true);
				}
			};
		});
	}
	
	
	Point mp;
	Image img;
	int ang = 0;
	
	public 레이더_애니메이션(ImageIcon img) {
		this.img = img.getImage();
		addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				ang = 0;
				mp = e.getPoint();
				Thread th = new Thread(()->{
					while(ang++<360) {
						repaint();
						try {
							Thread.sleep(1);
						} catch (InterruptedException e1) {
							e1.printStackTrace();
						}
					}
				});
				th.start();
			}
		});
	}
	
	@Override
	protected void paintComponent(Graphics g) {
		super.paintComponent(g);
		g.drawImage(img, 0, 0, null);
		if(mp!=null) {
			g.setColor(new Color(170, 200, 230, 50));
			g.fillOval(mp.x-50, mp.y-50, 100, 100);
			g.setColor(new Color(170, 170, 255, 200));
			g.fillArc(mp.x-50, mp.y-50, 100, 100, -90 + ang, 30);
		}
	}

}
