package db;

import java.util.Properties;
import java.sql.Connection;
import java.sql.DriverManager;
// import java.sql.SQLException;

import picocli.CommandLine;
import picocli.CommandLine.Option;
// import picocli.CommandLine.Parameters;


public class App  implements Runnable 
{
    @Option(names = { "-b", "--bean" }, description = "Use Bean export " )
    private boolean useBean = false;

    @Option(names = { "-rs", "--result-set" }, description = "Use ResultSet directly" )
    private boolean useResultSet = false;

    @Option(names = { "-h", "--help" }, usageHelp = true,
        description = "Displays this help message and quits.")
    private boolean helpRequested = false;

    
    public static void main( String[] args )
    {
        CommandLine.run(new App(), args);
    }

    String getVar(String key, String defaultValue){
        String ret = System.getenv(key);
        if (ret == null) ret = defaultValue;
        return ret;
    }

    public void run() {
        Connection conn;
        var csvFilename = "csvfile";
        try {
            Class.forName("org.postgresql.Driver");
            String pghost = getVar("PGHOST", "192.168.0.48");
            String pgdatabase = getVar("PGDATABASE", "bruno");
            String pguser = getVar("PGUSER", "bruno");
            String pgpassword = getVar("PGPASSWORD", "bruno");
            String url = String.format("jdbc:postgresql://%s/%s", pghost, pgdatabase);
            Properties props = new Properties();
            props.setProperty("user", pguser);
            props.setProperty("password", pgpassword);
            props.setProperty("ssl", "false");

            conn = DriverManager.getConnection(url, props);
            var exporter = new QueryExporter(conn);
            var rs = exporter.runQuery();
            if (useBean){
                exporter.beanExport(rs, csvFilename + "-bean.csv");
            }else{
                exporter.resultSetExport(rs, csvFilename + "-rs.csv");
            }
            conn.close();

        } catch (Exception e) {
            e.printStackTrace();
        }        
    }
}
