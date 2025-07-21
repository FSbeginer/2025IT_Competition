package 전남2;

import java.awt.Color;
import java.awt.Graphics;

import javax.swing.*;

public class ButtonToggle {
	JFrame jf = new JFrame();
	JPanel jp = new JPanel();
	MyToggle btn1, btn2;

	public ButtonToggle() {
		jf.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		jf.setSize(300, 100);

		btn1 = new MyToggle("����");
		btn2 = new MyToggle("���");
		btn1.setSelected(true);

		ButtonGroup grp1 = new ButtonGroup();
		grp1.add(btn1);
		grp1.add(btn2);

		jp.add(btn1);
		jp.add(btn2);
		jf.getContentPane().add(jp);
		jf.setVisible(true);
	}

	public static void main(String[] args) {
		new ButtonToggle();
	}

	class MyToggle extends JToggleButton {
		public MyToggle(String t) {
			setText(t);
			setContentAreaFilled(false);
			setFocusPainted(false);
		}

		@Override
		protected void paintComponent(Graphics g) {
			g.setColor(isSelected()?Color.GREEN:Color.GRAY);
			g.fillRect(0, 0, getWidth(), getHeight());

			g.setColor(Color.BLACK);
			g.drawRect(0, 0, getWidth(), getHeight());
			
			super.paintComponent(g);
		}
	}
}