package 전남2;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import java.awt.Color;
import javax.swing.JLabel;
import javax.swing.SwingConstants;
import javax.swing.Timer;

import java.awt.Font;
import java.time.Duration;
import java.time.LocalDate;
import java.time.LocalDateTime;

public class 시간표기 extends JFrame {

	private JPanel contentPane;
	public JLabel label;
	public JLabel label_1;
	public JLabel label_2;
	public JLabel label_3;
	public JLabel label_4;
	public JLabel label_5;
	public JLabel label_6;
	public JLabel label_7;
	JLabel[] jls;
	LocalDateTime end = LocalDateTime.now().plusHours(6);
	public JLabel label_8;
	
	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					시간표기 frame = new 시간표기();
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
	public 시간표기() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 736, 237);
		contentPane = new JPanel();
		contentPane.setBackground(new Color(135, 206, 235));
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		setContentPane(contentPane);
		contentPane.setLayout(null);
		
		label = new JLabel("");
		label.setFont(new Font("굴림", Font.BOLD, 30));
		label.setOpaque(true);
		label.setBackground(new Color(255, 255, 255));
		label.setHorizontalAlignment(SwingConstants.CENTER);
		label.setBounds(95, 55, 87, 85);
		contentPane.add(label);
		
		label_1 = new JLabel("");
		label_1.setOpaque(true);
		label_1.setHorizontalAlignment(SwingConstants.CENTER);
		label_1.setFont(new Font("굴림", Font.BOLD, 30));
		label_1.setBackground(Color.WHITE);
		label_1.setBounds(194, 55, 87, 85);
		contentPane.add(label_1);
		
		label_2 = new JLabel("");
		label_2.setOpaque(true);
		label_2.setHorizontalAlignment(SwingConstants.CENTER);
		label_2.setFont(new Font("굴림", Font.BOLD, 30));
		label_2.setBackground(Color.WHITE);
		label_2.setBounds(306, 55, 87, 85);
		contentPane.add(label_2);
		
		label_3 = new JLabel("");
		label_3.setOpaque(true);
		label_3.setHorizontalAlignment(SwingConstants.CENTER);
		label_3.setFont(new Font("굴림", Font.BOLD, 30));
		label_3.setBackground(Color.WHITE);
		label_3.setBounds(405, 55, 87, 85);
		contentPane.add(label_3);
		
		label_4 = new JLabel("");
		label_4.setOpaque(true);
		label_4.setHorizontalAlignment(SwingConstants.CENTER);
		label_4.setFont(new Font("굴림", Font.BOLD, 30));
		label_4.setBackground(Color.WHITE);
		label_4.setBounds(522, 55, 87, 85);
		contentPane.add(label_4);
		
		label_5 = new JLabel("");
		label_5.setOpaque(true);
		label_5.setHorizontalAlignment(SwingConstants.CENTER);
		label_5.setFont(new Font("굴림", Font.BOLD, 30));
		label_5.setBackground(Color.WHITE);
		label_5.setBounds(621, 55, 87, 85);
		contentPane.add(label_5);
		
		label_6 = new JLabel(":");
		label_6.setForeground(Color.WHITE);
		label_6.setFont(new Font("맑은 고딕", Font.BOLD, 33));
		label_6.setHorizontalAlignment(SwingConstants.CENTER);
		label_6.setBounds(279, 55, 30, 85);
		contentPane.add(label_6);
		
		label_7 = new JLabel(":");
		label_7.setHorizontalAlignment(SwingConstants.CENTER);
		label_7.setForeground(Color.WHITE);
		label_7.setFont(new Font("맑은 고딕", Font.BOLD, 33));
		label_7.setBounds(491, 55, 30, 85);
		contentPane.add(label_7);
		
		label_8 = new JLabel("New label");
		label_8.setBounds(12, 76, 63, 44);
		contentPane.add(label_8);
		
		jls = new JLabel[] {label, label_1, label_2, label_3, label_4, label_5};
		setTimer();
	}

	private void setTimer() {
		new Timer(1000, (e)->{
			Duration dur = Duration.between(LocalDateTime.now(), end);
			long tot = dur.getSeconds();
			
			long days = tot / (3600*24);
			long hours = (tot % (3600*24))/3600;
			long minutes = tot % 3600 / 60;
			long seconds = tot % 60;
			
			jls[0].setText(hours/10+"");
			jls[1].setText(hours%10+"");
			jls[2].setText(minutes/10+"");
			jls[3].setText(minutes%10+"");
			jls[4].setText(seconds/10+"");
			jls[5].setText(seconds%10+"");
			label_8.setText(String.format("%01d일", days));
		}).start();
	}

}
