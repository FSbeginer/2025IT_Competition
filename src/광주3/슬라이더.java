package 광주3;

import java.awt.Color;
import java.awt.Dimension;
import java.io.File;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.List;

import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JSlider;
import javax.swing.SwingUtilities;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;

public class 슬라이더 extends JFrame {
	
	List<JLabel> imgs = new ArrayList<JLabel>();
	int before = 0;
	
	public 슬라이더() {
		ui();
		setVisible(true);
	}
	
	private void ui() {
		setSize(800, 400);
		setDefaultCloseOperation(3);	
		setLocationRelativeTo(null);
		
		JPanel cp = new JPanel();
		cp.setLayout(null);
		cp.setBackground(Color.gray);
		add(cp);
		
		JSlider js = new JSlider();
		js.setValue(0);
		js.addChangeListener(new ChangeListener() {
			@Override
			public void stateChanged(ChangeEvent e) {
				for (JLabel jLabel : imgs) {
					jLabel.setLocation(jLabel.getX()-(js.getValue()-before), 0);
				}
				before = js.getValue();
			}
		});
		add(js,"South");
		
		int gap = 20, w = 140, h = 200;
		for (int i = 1; i <= 13; i++) {
			String path = new File("./광주3/movie/"+i+".jpg").exists()? "./광주3/movie/"+i+".jpg" :"./광주3/movie/"+i+".png"; 
			JLabel jl = new JLabel(new ImageIcon(new ImageIcon(path).getImage().getScaledInstance(w, h, 4)));
			jl.setBounds(gap+(w+gap)*(i-1), 0, w, h);
			
			imgs.add(jl);
			cp.add(jl);
		}
		
		js.setMaximum(gap+(w+gap)*13-getWidth());
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 슬라이더();});
	}
}
