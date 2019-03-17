using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

using Serilog;
// using Microsoft.EntityFrameworkCore;

using Npgsql;
using NpgsqlTypes;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;

namespace CmdlineApp
{

    public sealed class CollegeMap : ClassMap<College>{
        public CollegeMap(){
            Map(m => m.Id);
            Map(m => m.Name);
            Map(m => m.City);
            Map(m => m.State);
            Map(m => m.Zip);
            Map(m => m.Region);
            Map(m => m.Latitude);
            Map(m => m.Longitude);
            Map(m => m.SatAvg).TypeConverterOption.Format("##0");
            Map(m => m.ActAvg).TypeConverterOption.Format("##0");
            Map(m => m.AdmRate);
            Map(m => m.Earnings).TypeConverterOption.Format("##0");
            Map(m => m.Cost).TypeConverterOption.Format("##0");
            //In case of datetime column
            //  Map(m => m.Cost).TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
            Map(m => m.Enrollments).TypeConverterOption.Format("##0");
            Map(m => m.Info).Ignore();
            // 
        }
    }

    public class CsvExporter {
        string ConnectionString;
        decimal? ToDecimal(Object v){
            return (v is System.DBNull)? null: (decimal?)v;
        }
        public CsvExporter(string connString){
            ConnectionString = connString;
        }

        public int WriteToCsv(string query, string CsvFile){
            int total = 0;
            using (var conn = new NpgsqlConnection(ConnectionString)){
                conn.Open();
                using(var writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, CsvFile)))
                using(var csv = new CsvWriter(writer))
                using(var cmd = new NpgsqlCommand(query, conn))
                using(var reader = cmd.ExecuteReader()){
                    csv.Configuration.RegisterClassMap<CollegeMap>();
                    csv.Configuration.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = 
                        new string[]{"yyyy-MM-dd HH:mm:ss"};
                    while(reader.Read()){
                        Object[] values = new Object[reader.FieldCount];
                        int fieldCount = reader.GetValues(values);
                        College item = new College{
                            Id = (string)values[0], Name = (string)values[1],City = (string)values[2],State = (string)values[3],
                            Zip = (string)values[4],Region = (int)values[5], 
                            Latitude = ToDecimal(values[6]), Longitude = ToDecimal(values[7]),
                            AdmRate = ToDecimal(values[8]), SatAvg = ToDecimal(values[9]), ActAvg=ToDecimal(values[10]),
                            Earnings=ToDecimal(values[11]), Cost=ToDecimal(values[12]),Enrollments=ToDecimal(values[13])
                        };
                        csv.WriteRecord(item);
                        csv.NextRecord();
                        total ++;
                        if (total % 100 == 0){
                            Console.WriteLine($"{total} exported ...");
                        }
                    }
                }
            }
            return total;
        }

    }
}