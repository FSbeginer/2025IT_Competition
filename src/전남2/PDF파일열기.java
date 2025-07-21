package 전남2;

import java.awt.Desktop;
import java.io.File;
import java.io.IOException;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.SwingUtilities;

public class PDF파일열기 extends JFrame {

	public PDF파일열기() {
		ui();

		setVisible(true);
	}

	private void ui() {
		setSize(400, 600);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		add(new JButton("PDF열기") {
			{
				try {
					Desktop.getDesktop().open(new File("./전남2/question/6시그마/1.pdf"));
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		});
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(PDF파일열기::new);
	}
}
