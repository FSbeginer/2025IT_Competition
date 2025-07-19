package 전남2_보류;

import java.awt.Dimension;
import java.awt.GridLayout;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import javax.swing.BoxLayout;
import javax.swing.ButtonGroup;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 레이블애니메이션_하다맘 extends JFrame {

	private JPanel jp;
	JLabel[] jls;
	List<String> data = new ArrayList<String>();

	public 레이블애니메이션_하다맘() {
		ui();
		addPanel();
		addLabel();
		setVisible(true);
	}

	private void addLabel() {
		try (var rs = Connect.res("")) {
			
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}

	private void addPanel() {
		jp = new JPanel() {{setLayout(new BoxLayout(this, BoxLayout.Y_AXIS));}};
		add(jp);
	}

	private void ui() {
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
		setSize(500, 500);

		add(new JPanel() {
			{
				ButtonGroup bg = new ButtonGroup();
				setPreferredSize(new Dimension(0, 50));
				setLayout(new GridLayout(1, 2, 5, 0));
				add(new RoundButton("추천순") {{bg.add(this); setSelected(true);}});
				add(new RoundButton("별점순") {{bg.add(this);}});
			}
		}, "North");
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(레이블애니메이션_하다맘::new);
	}
}
