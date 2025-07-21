package 전남2;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.util.Arrays;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 차트애니메이션 extends JFrame {
	
	int[] data = {12, 4, 5, 1, 9};
	Color[] c = {Color.red, Color.green, Color.blue, Color.pink, Color.orange};
	Color target;
	BufferedImage img;
	int rotate = 0;
	JLabel jl = new JLabel() {{setSize(100, 100);}};
	
	public 차트애니메이션() {
		ui();
		getData();
		setVisible(true);
	}
	
	private void getData() {
		int sum = Arrays.stream(data).sum();
		for (int i = 0; i < data.length; i++) {
			data[i] = (int) ((double)data[i] / sum * 365);
		}
	}

	private void ui() {
		setSize(500, 500);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		
		JPanel jp = new JPanel(null) {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);

				img = new BufferedImage(getWidth(), getHeight(), 2);
				Graphics2D g2d = img.createGraphics();
				
				if(rotate!=0) {
					g2d.rotate(Math.toRadians(rotate++), 250, 250);
					rotate %= 360;
				}
				
				int start = 0;
				for (int i = 0; i < 5; i++) {
					g2d.setColor(c[i]);
					if(target!=null && rotate==0 && c[i].equals(target)) {
						int moveX = (int) (Math.cos(Math.toRadians((start*2+data[i])/2))*20);
						int moveY = -(int) (Math.sin(Math.toRadians((start*2+data[i])/2))*20);
						g2d.fillArc(50+moveX, 50+moveY, 400, 400, start, data[i]);
						jl.setToolTipText((i+1)+"번째 비율: "+String.format("%.1f%%", data[i]/360.0*100));
					}
					else {
						g2d.fillArc(50, 50, 400, 400, start, data[i]);
					}
					start += data[i];
				}
				
				g.drawImage(img, 0, 0, null);
				try {
					Thread.sleep(2);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				repaint();
			}
		};
		jp.addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				Color selColor = new Color(img.getRGB(e.getX(), e.getY()));
				target = Arrays.stream(c).filter(x->x.equals(selColor)).findAny().orElse(null);
				if(target!=null) rotate = 1;
			}
		});
		jp.addMouseMotionListener(new MouseAdapter() {
			public void mouseMoved(MouseEvent e) {
				Color c = new Color(img.getRGB(e.getX(), e.getY()));
				if(c.equals(target)) jl.setLocation(e.getX()-50, e.getY()-50);
			};
		});
		jp.add(jl);
		add(jp);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(차트애니메이션::new);
	}
}
