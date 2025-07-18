package 충남3;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

public class Connect {
	static Connection con;
	static Statement stmt;
	
	static {
		try {
			con = DriverManager.getConnection("jdbc:mysql://localhost?serverTimezone=UTC", "root", "1234");
			stmt = con.createStatement();
			stmt.execute("use hospital");
		} catch (SQLException e) {
			e.printStackTrace();
		}
	}
	
	public static PreparedStatement pre(String sql) throws SQLException {
		return con.prepareStatement(sql);
	}
	public static void preSet(PreparedStatement pre, Object...objects) throws SQLException {
		int i = 1;
		for (Object object : objects) {
			pre.setObject(i++, pre);
		}
	}
	public static ResultSet res(String sql) throws SQLException {
		return pre(sql).executeQuery();
	}
	public static void execute() throws SQLException {
		stmt.execute("use moviedb");
	}
}
