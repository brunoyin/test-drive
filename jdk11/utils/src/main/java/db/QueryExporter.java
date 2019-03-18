/*
 * Created on Sun Mar 17 2019
 *
 * Copyright (c) 2019 bruno.yin@gmail.com
 */

package db;

import java.sql.*;
// import java.util.Properties;
import java.util.List;
import java.util.ArrayList;

import com.opencsv.CSVWriter;
import com.opencsv.ResultSetHelper;
import com.opencsv.ResultSetHelperService;
import com.opencsv.bean.ColumnPositionMappingStrategy;
import com.opencsv.bean.StatefulBeanToCsv;
import com.opencsv.bean.StatefulBeanToCsvBuilder;

import java.io.*;

public class QueryExporter{
    static final String CSV_DATE_FORMAT = "yyyy-MM-dd";
    static final String CSV_TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

    Connection conn;

    public QueryExporter(Connection conn){
        this.conn = conn;
    }

    public ResultSet runQuery() throws SQLException{
        conn.setAutoCommit(false);
        Statement st = conn.createStatement();
        st.setFetchSize(50);
        ResultSet rs = st.executeQuery("select * from yin.college limit 200");        
        // System.out.println("Done");
        return rs;
    }

    public void beanExport(ResultSet rs, String csvFilename){
        List<College>results = new ArrayList<College>();
        try {
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
                results.add(item);
            }
            rs.close();

            FileWriter fw = new FileWriter(csvFilename);
            ColumnPositionMappingStrategy<College> mappingSrategy = new ColumnPositionMappingStrategy<College>();
            mappingSrategy.setType(College.class);
            String[] columns = new String[]{
                "Id", "Name", "City", "State", "Zip" , "Region","Latitude", "Longitude",  "AdmRate", 
                "SatAvg", "ActAvg", "Earnings", "Cost", "Enrollments"
            };
            mappingSrategy.setColumnMapping(columns);
            StatefulBeanToCsvBuilder<College> builder = new StatefulBeanToCsvBuilder<College>(fw);
            StatefulBeanToCsv<College> beanWriter = builder.withMappingStrategy(mappingSrategy).build();
            beanWriter.write(results);
            
            fw.close();
            
            System.out.println("done: " + csvFilename);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public void resultSetExport(ResultSet rs, String csvFilename) throws SQLException, IOException {
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

        System.out.println("done: " + csvFilename);
    }
}