package 경북2;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.RenderingHints;

import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 배송정보 extends JFrame {

	Image[] images = new Image[7];
	String[] names = {"결제완료", "배송준비", "배송중", "배송완료"};
	int[] idxs = { 0, 1, 3, 5 };
	int w, h, act = 2;
	private JPanel jp;

	public 배송정보() {
		ui();
		addPanel();
		getData();
		ani();
		setVisible(true);
	}

	private void ani() {
		Thread th = new Thread(new Runnable() {
			@Override
			public void run() {
				int idx = idxs[act];
				while(true) {
					try {
						idxs[act] = -1;
						Thread.sleep(1000);
						repaint();
						idxs[act] = idx;
						Thread.sleep(1000);
						repaint();
					} catch (InterruptedException e) {
						e.printStackTrace();
					}
				}
			}
		});
		th.start();
	}

	private void getData() {
		int idx = 0;
		w = jp.getWidth() / 8;
		h = w;
		for (int i = 0; i < 4; i++) {
			for (int j = i == 0 ? 2 : 1; j <= 2; j++) {
				images[idx++] = new ImageIcon(new ImageIcon("./경북2/delivery/" + i + j + ".jpg").getImage().getScaledInstance(w, h, 4)).getImage();
			}
		}
		for (int i = 1; i <= act; i++) {
			idxs[i]++;
		}
	}

	private void addPanel() {
		jp = new JPanel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				Graphics2D g2d = (Graphics2D) g;
				g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
				g2d.setStroke(new BasicStroke(3));
				
				for (int i = 3; i >= 0; i--) {
					Color c = act>=i ? Color.blue.brighter() : Color.LIGHT_GRAY;
					g2d.setColor(c);
					g2d.drawLine(w*2*Math.max(i-1, 0)+w/2, w+10, w*2*i+w/2, w+10);
					g2d.fillOval(w*2*i+(w/2)-5, w+10-5, 10, 10);
					
					if(idxs[i]==-1) continue;
					g2d.drawImage(images[idxs[i]], w * 2 * i, 0, null);
					int fx = w * 2 * i + 10;
					g2d.drawString(names[i], fx, w+50);
				}
				
			}
		};
		jp.setSize(400, 150);
		jp.setBackground(Color.white);
		add(jp);
	}

	private void ui() {
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		setSize(400, 150);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(() -> {
			new 배송정보();
		});
	}
}
