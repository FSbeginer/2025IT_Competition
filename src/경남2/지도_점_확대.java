package 경남2;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.Point;
import java.awt.RenderingHints;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class 지도_점_확대 extends JFrame {
	
	JLabel map;
	List<Point> plist = new ArrayList<Point>();
	Point clickP;
	double scale;
	
	public 지도_점_확대() {
		ui();
		getdata();
		setVisible(true);
	}
	
	private void getdata() {
		try (var rs = Connect.res("select * from brand")) {
			while(rs.next()) {
				plist.add(new Point(rs.getInt("bxx"), rs.getInt("byy")));
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	private void ui() {
		setSize(600, 550);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		
		map = new JLabel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				Graphics2D g2d = (Graphics2D) g;
				g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);

				if(clickP!=null) {
					// (x,y)를 (0,0)으로 잡기위해 -x,-y로 이동 후 화면 중앙으로 위치
					g2d.translate(getWidth() / 2.0 - scale * clickP.x, getHeight() / 2.0 - scale * clickP.y );
					g2d.scale(scale, scale);	
				}
				
				Image img = new ImageIcon(new ImageIcon("./경남2/지도.png").getImage().getScaledInstance(600, 550, 4)).getImage();
				g2d.drawImage(img, 0, 0, null);
				
					
				g2d.setColor(Color.red);
				for (Point point : plist) {
					g2d.fillOval(point.x-4, point.y-4, 8, 8);
				}
			}
		};
		
		add(map);
		
		map.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				super.mouseClicked(e);
				for (Point point : plist) {
					if(e.getPoint().distance(point) < 10) {
						clickP = e.getPoint();
						zoom();
					}
				}
			}

			private void zoom() {
				Thread th = new Thread(new Runnable() {
					@Override
					public void run() {
						for (int i = 2; i <= 16; i++) {
							scale = 0.5*i;
							repaint();
							try {
								Thread.sleep(100);
							} catch (InterruptedException e) {
							}
						}
					}
				});
				th.start();
			}
		});
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 지도_점_확대();});
	}
}
