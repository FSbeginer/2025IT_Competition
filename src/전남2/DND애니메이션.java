package 전남2;

import java.awt.EventQueue;
import java.awt.Image;
import java.awt.datatransfer.DataFlavor;
import java.awt.datatransfer.UnsupportedFlavorException;
import java.awt.dnd.DnDConstants;
import java.awt.dnd.DropTarget;
import java.awt.dnd.DropTargetAdapter;
import java.awt.dnd.DropTargetDropEvent;

import javax.imageio.ImageIO;
import javax.swing.ImageIcon;
import javax.swing.JFileChooser;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import java.awt.Color;
import javax.swing.JLabel;
import javax.swing.border.LineBorder;
import javax.swing.filechooser.FileNameExtensionFilter;
import javax.swing.SwingConstants;
import javax.swing.SwingUtilities;

import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;
import java.util.List;

public class DND애니메이션 extends JFrame {

	private JPanel contentPane;
	public JLabel label;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					DND애니메이션 frame = new DND애니메이션();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 */
	public DND애니메이션() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 743, 475);
		contentPane = new JPanel();
		contentPane.setBackground(Color.WHITE);
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		setContentPane(contentPane);
		contentPane.setLayout(null);
		
		label = new JLabel("이미지 DND or Click");
		label.addMouseListener(new LabelMouseListener());
		label.setHorizontalAlignment(SwingConstants.CENTER);
		label.setBorder(new LineBorder(new Color(0, 0, 0)));
		label.setBounds(12, 43, 323, 307);
		contentPane.add(label);
		
		new DropTarget(label, new DropTargetAdapter() {
			@Override
			public void drop(DropTargetDropEvent dtde) {
				dtde.acceptDrop(DnDConstants.ACTION_COPY);
				var tf = dtde.getTransferable();
				try {
					List<File> list = ((List<File>)tf.getTransferData(DataFlavor.javaFileListFlavor));
					File f = list.get(0);
					var path = f.getAbsolutePath();
					if(path.toLowerCase().contains(".png")) {
						var img = ImageIO.read(f).getScaledInstance(label.getWidth(), label.getHeight(), 4);
						setIconAni(img);
					}
				} catch (UnsupportedFlavorException | IOException e) {
					e.printStackTrace();
				}
			}
		});
	}
	
	private void setIconAni(Image img) {
		BufferedImage ori = new BufferedImage(img.getWidth(null), img.getHeight(null), 2);
		
		var g2d = ori.createGraphics();
		g2d.drawImage(img, 0, 0, null);
		
		BufferedImage dis = new BufferedImage(img.getWidth(null), img.getHeight(null), 2);
		Thread th = new Thread(()->{
			for (int i = 0; i < label.getWidth()*2; i++) {
				for (int y = 0; y < ori.getHeight(); y++) {
					for (int x = 0; x < ori.getWidth(); x++) {
						if(x+y!=i) continue;
						int argb = ori.getRGB(x, y);
						dis.setRGB(x, y, argb);
					}
				}
				SwingUtilities.invokeLater(()->{label.setIcon(new ImageIcon(dis));});
				try {
					Thread.sleep(5);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
		});
		th.start();
	}
	
	private class LabelMouseListener extends MouseAdapter {
		@Override
		public void mouseClicked(MouseEvent e) {
			JFileChooser jfc = new JFileChooser();
			jfc.setAcceptAllFileFilterUsed(false);
			jfc.setMultiSelectionEnabled(false);
			var filter = new FileNameExtensionFilter("Png Image File", "png");
			jfc.setFileFilter(filter);
			if(jfc.showOpenDialog(null)==jfc.APPROVE_OPTION) {
				try {
					setIconAni(ImageIO.read(jfc.getSelectedFile()).getScaledInstance(label.getWidth(), label.getHeight(), 4));
				} catch (IOException e1) {
					e1.printStackTrace();
				}
			}
		}
	}
}
