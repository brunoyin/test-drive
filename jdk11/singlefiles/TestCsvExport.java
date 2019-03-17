import java.sql.*;
import java.util.Properties;
import java.util.List;
import java.util.ArrayList;

import com.opencsv.CSVWriter;
import com.opencsv.ResultSetHelper;
import com.opencsv.ResultSetHelperService;
import com.opencsv.bean.ColumnPositionMappingStrategy;
import com.opencsv.bean.StatefulBeanToCsv;
import com.opencsv.bean.StatefulBeanToCsvBuilder;

import java.io.*;

public class TestCsvExport{
    static final String CSV_DATE_FORMAT = "yyyy-MM-dd";
    static final String CSV_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

    public static class College{
        String Id, Name, City, State, Zip;
        int Region;
        Double Latitude, Longitude,  AdmRate, SatAvg, ActAvg, Earnings, Cost, Enrollments;

        
	public String getId() {
		return Id;
	}
	public void setId(String id) {
		Id = id;
	}
	public String getName() {
		return Name;
	}
	public void setName(String name) {
		Name = name;
	}
	public String getCity() {
		return City;
	}
	public void setCity(String city) {
		City = city;
	}
	public String getState() {
		return State;
	}
	public void setState(String state) {
		State = state;
	}
	public String getZip() {
		return Zip;
	}
	public void setZip(String zip) {
		Zip = zip;
	}
	public int getRegion() {
		return Region;
	}
	public void setRegion(int region) {
		Region = region;
	}
	public Double getLatitude() {
		return Latitude;
	}
	public void setLatitude(Double latitude) {
		Latitude = latitude;
	}
	public Double getLongitude() {
		return Longitude;
	}
	public void setLongitude(Double longitude) {
		Longitude = longitude;
	}
	public Double getAdmRate() {
		return AdmRate;
	}
	public void setAdmRate(Double admRate) {
		AdmRate = admRate;
	}
	public Double getSatAvg() {
		return SatAvg;
	}
	public void setSatAvg(Double satAvg) {
		SatAvg = satAvg;
	}
	public Double getActAvg() {
		return ActAvg;
	}
	public void setActAvg(Double actAvg) {
		ActAvg = actAvg;
	}
	public Double getEarnings() {
		return Earnings;
	}
	public void setEarnings(Double earnings) {
		Earnings = earnings;
	}
	public Double getCost() {
		return Cost;
	}
	public void setCost(Double cost) {
		Cost = cost;
	}
	public Double getEnrollments() {
		return Enrollments;
	}
	public void setEnrollments(Double enrollments) {
		Enrollments = enrollments;
	}

    }

    public static void main(String[] args) throws ClassNotFoundException, SQLException, IOException {
     
        Class.forName("org.postgresql.Driver");
        String pghost = "192.168.0.48";
        String url = String.format("jdbc:postgresql://%s/bruno", pghost);
        Properties props = new Properties();
        props.setProperty("user", "bruno");
        props.setProperty("password", "bruno");
        props.setProperty("ssl", "false");

        Connection conn = DriverManager.getConnection(url, props);

        conn.setAutoCommit(false);
        Statement st = conn.createStatement();
        st.setFetchSize(50);
        ResultSet rs = st.executeQuery("select * from yin.college limit 200");
        List<College> collegeList = new ArrayList<College>();
        while(rs.next()){
            College item = new College();
            item.Id = rs.getString(1);
            item.Name = rs.getString(2);
            item.City = rs.getString(3);
            item.State = rs.getString(4);
            item.Zip = rs.getString(5);
            item.Region = rs.getInt(6);
            item.Latitude = rs.getDouble(7);
            item.Longitude = rs.getDouble(8);
            item.AdmRate = rs.getDouble(9);
            item.SatAvg = rs.getDouble(10);
            item.ActAvg = rs.getDouble(11);
            item.Earnings = rs.getDouble(12);
            item.Cost = rs.getDouble(13);
            item.Enrollments = rs.getDouble(14);
            collegeList.add(item);
        }
        rs.close();
        System.out.println("Total rows exported = " + collegeList.size());
        // 
        beanExport(collegeList, "beanExport.csv");
        rs = st.executeQuery("select * from yin.college limit 250");
        export(rs, "csvfile.csv");
        rs.close();
        conn.close();
        System.out.println("Done");
    }

    public static void beanExport(List<College>results, String csvFilename){
        try {
            FileWriter fw = new FileWriter(csvFilename);
            ColumnPositionMappingStrategy mappingSrategy = new ColumnPositionMappingStrategy();
            mappingSrategy.setType(College.class);
            String[] columns = new String[]{
                "Id", "Name", "City", "State", "Zip" , "Region","Latitude", "Longitude",  "AdmRate", 
                "SatAvg", "ActAvg", "Earnings", "Cost", "Enrollments"
            };
            mappingSrategy.setColumnMapping(columns);
            StatefulBeanToCsvBuilder<College> builder = new StatefulBeanToCsvBuilder(fw);
            StatefulBeanToCsv beanWriter = builder.withMappingStrategy(mappingSrategy).build();
            beanWriter.write(results);
            // beanWriter.close();
            fw.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static void export(ResultSet rs, String csvFilename) throws SQLException, IOException {
        ResultSetHelperService resultSetHelper = new ResultSetHelperService();
        resultSetHelper.setDateFormat(CSV_DATE_FORMAT);
        resultSetHelper.setDateTimeFormat(CSV_TIMESTAMP_FORMAT);
        FileWriter fw = new FileWriter(csvFilename);
        CSVWriter writer = new CSVWriter(
            fw, ',', CSVWriter.NO_QUOTE_CHARACTER,
            CSVWriter.DEFAULT_ESCAPE_CHARACTER,
            CSVWriter.DEFAULT_LINE_END
        );
        writer.setResultService((ResultSetHelper)resultSetHelper);
        writer.writeAll(rs, true);
        writer.close();
        fw.close();
    }
}