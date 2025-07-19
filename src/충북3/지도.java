 package 충북3;

import java.awt.BasicStroke;
import java.awt.Color;
import java.awt.Font;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.Point;
import java.awt.RenderingHints;
import java.awt.image.BufferedImage;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;

import javax.swing.ImageIcon;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.SwingUtilities;

public class 지도 extends JFrame {

	int[] dist = new int[12];
	int[] prev = new int[12];
	List<Location> locations = new ArrayList<지도.Location>();
	List<Edge> connections = new ArrayList<지도.Edge>();
	ArrayList<Integer> route;
	BufferedImage lineImg = new BufferedImage(200, 200, 2);
	
	public 지도() {
		ui();
		getData(12, 3); // 1~12사이로
		map();
		ani();
		setVisible(true);
	}
	
	private void ani() {
		lineImg = new BufferedImage(getWidth(), getHeight(), 2);
		
		Thread th = new Thread(()->{
			Graphics2D g2d = lineImg.createGraphics();
			g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
			g2d.setStroke(new BasicStroke(4));
			g2d.setColor(Color.red);
			for (int i = 0; i < route.size()-1; i++) {
				var from = locations.get(route.get(i)).p;
				var to = locations.get(route.get(i+1)).p;
				g2d.drawLine(from.x, from.y, to.x, to.y);
				repaint();
				try {
					if(i!=route.size()-2) Thread.sleep((long) (from.distance(to)/5)*100);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
			}
			System.out.println("도착");
		});
		th.start();
	}

	private void getData(int start, int end) {
		try (var rs = Connect.res("select * from area")) {
			while(rs.next()) {
				locations.add(new Location(rs.getString(2), new Point(rs.getInt(3),rs.getInt(4))));
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
		try (var rs = Connect.res("select * from connect")) {
			while(rs.next()) {
				connections.add(new Edge(locations.get(rs.getInt(1)-1), locations.get(rs.getInt(2)-1)));
				connections.add(new Edge(locations.get(rs.getInt(2)-1), locations.get(rs.getInt(1)-1)));
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
		
		Arrays.fill(dist, Integer.MAX_VALUE);
		Arrays.fill(prev, -1);
		dist[start-1] = 0;
		
		for (int i = 0; i < 11; i++) {
			for (Edge edge : connections) {
				int from = locations.indexOf(edge.from);
				int to = locations.indexOf(edge.to);
				if(dist[from]+edge.dist < dist[to] && dist[from] != Integer.MAX_VALUE) {
					dist[to] = (int) (dist[from] + edge.dist);
					prev[to] = from;
				}
			}
		}
		
		route = new ArrayList<Integer>();
		route.add(end-1);
		int from = prev[end-1]; 
		while(from!=-1) {
			route.add(from);
			from = prev[from];
		}
		Collections.reverse(route);
	}

	private void map() {
		JPanel jp = new JPanel() {
			@Override
			protected void paintComponent(Graphics g) {
				super.paintComponent(g);
				Graphics2D g2d = (Graphics2D) g;
				g2d.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
				g2d.setStroke(new BasicStroke(4));
				g2d.setFont(new Font("맑은 고딕", Font.BOLD, 12));
				
				Image img = new ImageIcon(new ImageIcon("./충북3/대한민국.png").getImage().getScaledInstance(getWidth(), getHeight(), 4)).getImage();
				g2d.drawImage(img, 0, 0, this);
				
				for (Edge edge : connections) {
					var from = edge.from.p;
					var to = edge.to.p;
					g2d.drawLine(from.x, from.y, to.x, to.y);
				}
				g2d.drawImage(lineImg, 0, 0, null);
				for (Location loc : locations) {
					g2d.setColor(Color.gray.brighter());
					g2d.fillOval(loc.p.x-6, loc.p.y-6, 12, 12);
					g2d.setColor(Color.LIGHT_GRAY.brighter());
					g2d.fillOval(loc.p.x-3, loc.p.y-3, 6, 6);
					g2d.setColor(Color.red);
					g2d.drawString(loc.name, loc.p.x+5, loc.p.y+10);
				}
			}
		};
		add(jp);
	}

	private void ui() {
		setSize(800, 778);
		setLocationRelativeTo(null);
		setDefaultCloseOperation(3);
	}

	public static void main(String[] args) {
		SwingUtilities.invokeLater(지도::new);
	}
	
	class Edge {
		Location from, to;
		double dist;
		public Edge(Location from, Location to) {
			this.from = from;
			this.to = to;
			dist = from.p.distance(to.p);
		}
	}
	class Location {
		String name;
		Point p;
		public Location(String name, Point p) {
			this.name = name;
			this.p = p;
		}
	}
}
