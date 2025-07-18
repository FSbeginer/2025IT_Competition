package 전남2_보류;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Graphics;

import javax.swing.JLabel;
import javax.swing.JRadioButton;
import javax.swing.SwingConstants;

public class RoundButton extends JRadioButton {
	JLabel jl;
	
	public RoundButton(String txt) {
		setLayout(new BorderLayout());
		add(jl = new JLabel(txt) {{setHorizontalAlignment(SwingConstants.CENTER);}});
	}

	@Override
	protected void paintComponent(Graphics g) {
		super.paintComponent(g);
		jl.setForeground(isSelected()? Color.white : Color.lightGray);
		g.setColor(isSelected()? Color.blue : Color.white);
		g.fillRoundRect(0, 0, getWidth() - 1, getHeight() - 1, 20, 20);
		g.setColor(isSelected()? Color.white : Color.LIGHT_GRAY);
		g.drawRoundRect(0, 0, getWidth() - 3, getHeight() - 3, 20, 20);
	}
}
