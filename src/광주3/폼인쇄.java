package 광주3;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.MouseAdapter;
import java.awt.event.MouseEvent;
import java.awt.print.PageFormat;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;

import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.SwingUtilities;

public class 폼인쇄 extends JFrame {

	public 폼인쇄() {
		ui();
		PrinterJob job = PrinterJob.getPrinterJob();
		job.setPrintable(new Printable() {
			@Override
			public int print(Graphics graphics, PageFormat pf, int pageIndex) throws PrinterException {
				if(pageIndex>0) return NO_SUCH_PAGE;
				Graphics2D g2d = (Graphics2D) graphics;
				g2d.translate(pf.getImageableX(), pf.getImageableY());
				폼인쇄.this.printAll(g2d); 
				return PAGE_EXISTS;
			}
		});
		addMouseListener(new MouseAdapter() {
			@Override
			public void mouseClicked(MouseEvent e) {
				super.mouseClicked(e);
				try {
					if(job.printDialog())
						job.print();
				} catch (PrinterException e1) {
					e1.printStackTrace();
				}
			}
		});
		add(new JButton(), "North");
		setVisible(true);
	}
	
	private void ui() {
		setSize(600, 400);
		setDefaultCloseOperation(3);
		setLocationRelativeTo(null);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(()->{new 폼인쇄();});
	}
}
