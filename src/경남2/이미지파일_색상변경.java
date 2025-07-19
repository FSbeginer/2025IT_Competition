package 경남2;

import java.awt.Color;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class 이미지파일_색상변경 extends JFrame {
	
	public 이미지파일_색상변경() {
		ui();
		addPng();
		setVisible(true);
	}
	
	private void addPng() {
		try {
			BufferedImage img = ImageIO.read(new File("./경남2/icon/insert.png"));
			for (int y = 0; y < img.getHeight(); y++) {
				for (int x = 0; x < img.getWidth(); x++) {
					if(img.getRGB(x, y)==Color.white.getRGB()) continue;
					img.setRGB(x, y, Color.RED.getRGB());
				}
			}
			
			JLabel jl = new JLabel(new ImageIcon(img));
			jl.setSize(400, 400);
			add(jl);
		} catch (IOException e) {
		}
	}

	private void ui() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setLocationRelativeTo(null);
		setSize(400, 400);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(이미지파일_색상변경::new);
	}
}
