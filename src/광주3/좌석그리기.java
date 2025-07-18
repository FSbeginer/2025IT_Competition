package 광주3;

import java.awt.Color;
import java.awt.Graphics;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class 좌석그리기 extends JFrame {

	public 좌석그리기() {
		ui();
	}
	
	private void ui() {
		setSize(200, 200);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		
		add(new SeatLabel(20, Color.red));
		setVisible(true);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 좌석그리기();});
	}
	
	class SeatLabel extends JLabel {
		int r;
		Color c;
		public SeatLabel(int r, Color c) {
			this.r = r;
			this.c = c;
		}
		@Override
		protected void paintComponent(Graphics g) {
			super.paintComponent(g);
			g.setColor(c);
			g.fillArc(0, 0, r*2, r*2, 0, 180);
			g.fillRect(0, r, r*2, r*2);
		}
	}
}
