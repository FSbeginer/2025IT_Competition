package 전남2_보류;

import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.print.PageFormat;
import java.awt.print.Printable;
import java.awt.print.PrinterException;
import java.awt.print.PrinterJob;

import javax.print.PrintServiceLookup;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.SwingUtilities;

public class PDF저장 extends JFrame {

	private PrinterJob job;

	public PDF저장() {
		ui();
		
		printer();

		setVisible(true);
	}

	private void printer() {
		job = PrinterJob.getPrinterJob();
		job.setPrintable(new Printable() {
			
			@Override
			public int print(Graphics graphics, PageFormat pageFormat, int pageIndex) throws PrinterException {
				if (pageIndex > 0)
					return NO_SUCH_PAGE;
				Graphics2D g2 = (Graphics2D) graphics;
				g2.translate(pageFormat.getImageableX(), pageFormat.getImageableY());
				PDF저장.this.printAll(g2);
				return 0;
			}
		});
		for (var ser : PrintServiceLookup.lookupPrintServices(null, null)) {
			if(ser.getName().equalsIgnoreCase("microsoft print to pdf")) {
				try {
					job.setPrintService(ser);
				} catch (PrinterException e) {
					e.printStackTrace();
				}
				break;
			}
		}
	}

	private void ui() {
		setLocationRelativeTo(null);
		setDefaultCloseOperation(3);
		setSize(400, 600);
		add(new JLabel("tq"), "North");
		add(new JButton("tqPDF변환") {
			{
				addActionListener(new ActionListener() {
					@Override
					public void actionPerformed(ActionEvent e) {
						if(job.printDialog()) {
							try {
								job.print();
							} catch (PrinterException e1) {
								e1.printStackTrace();
							}
						}
					}
				});
			}
		}, "South");
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(PDF저장::new);
	}
}
