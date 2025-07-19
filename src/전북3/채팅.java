package 전북3;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.FlowLayout;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.Date;

import javax.swing.BoxLayout;
import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import javax.swing.SwingUtilities;
import javax.swing.border.LineBorder;

public class 채팅 extends JFrame {

	public 채팅() {
		new JFrame() {
			{
				setSize(600, 600);
				setLocationRelativeTo(null);
				setDefaultCloseOperation(3);
				
				JPanel jp = new JPanel();
				jp.setLayout(new BoxLayout(jp, BoxLayout.Y_AXIS));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 0)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 0)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 0)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 0)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 0)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				jp.add(new ChatPanel(new Chat("안녕하세요", LocalDate.now(), 2)));
				add(jp);
				setVisible(true);
			}
		};
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(() -> {
			{
				new 채팅();
			}
		});
	}
	
	class Chat {
		String txt;
		LocalDate time;
		int horizon;
		public Chat(String txt, LocalDate time, int h) {
			this.txt = txt;
			this.time = time;
			horizon = h;
		}
	}
	class ChatPanel extends JPanel {
		JTextArea area;
		JLabel lblImg;
		JLabel lblTime;
		
		public ChatPanel(Chat chat) {
			setLayout(new FlowLayout(chat.horizon));
			setOpaque(false);
			
			SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH시 mm분");
			String date = format.format(new Date());
			
			lblImg = new JLabel(new ImageIcon(new ImageIcon("./전북3/logo.png").getImage().getScaledInstance(20, 20, 4)));
			lblTime = new JLabel(date);
			lblTime.setVerticalAlignment(JLabel.BOTTOM);
			
			area = new JTextArea(chat.txt);
			area.setLineWrap(true);
			area.setWrapStyleWord(true);
			area.setEditable(false);
			area.setBorder(new LineBorder(Color.black));
			area.setSize(200, Short.MAX_VALUE);
			Dimension pref = area.getPreferredSize();
			
			var fm = area.getFontMetrics(area.getFont());
			
			int txtW = fm.stringWidth(chat.txt)+20;
			int w = Math.min(200, txtW);
			area.setPreferredSize(new Dimension(w, pref.height));
			lblTime.setPreferredSize(new Dimension(fm.stringWidth(date)+20, pref.height));
			if(chat.horizon==0) {
				add(lblImg);
				add(area);
				add(lblTime);
			}
			else {
				add(lblTime);
				add(area);
				add(lblImg);
			}
		}
	}
}
