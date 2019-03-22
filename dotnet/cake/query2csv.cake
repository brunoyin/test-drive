// dotnet add package Npgsql --version 4.0.5
#addin nuget:?package=Npgsql&version=4.0.5
// dotnet add package CsvHelper --version 12.1.2
// #addin nuget:?package=CsvHelper&version=12.1.2
// dotnet add package System.Runtime.CompilerServices.Unsafe --version 4.5.2
#addin nuget:?package=System.Runtime.CompilerServices.Unsafe&version=4.5.2

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Runtime;
using System.Collections.Generic;

using Npgsql;
// using CsvHelper;
using NpgsqlTypes;

public partial class College
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public int? Region { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal? AdmRate { get; set; }
    public decimal? SatAvg { get; set; }
    public decimal? ActAvg { get; set; }
    public decimal? Earnings { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Enrollments { get; set; }

    public string Info {
        get{
            return $"{this.Id}: {this.Name}, costs ${this.Cost:#,##0}, average earnings in 10 years ${this.Earnings:#,##0}";
    }}
}

decimal? ToDecimal(Object v){
    return (v is System.DBNull)? null: (decimal?)v;
}

public async Task<int>Run(string val, bool queryByName, string ConnectionString){
    int ret = 0;
    // var clsName = this.GetType().Name;
    // Log.Information($"{clsName}.Run .... ");
    using(var cn = new NpgsqlConnection(ConnectionString)){
        string sqltext = (!queryByName) ? "select * from yin.college where id = @p1"
            :"select * from yin.college where earnings is not null and name ~* @p1 order by earnings desc limit 20";
        await cn.OpenAsync();
        var cmd = new NpgsqlCommand(sqltext, cn);
        var p1 = cmd.Parameters.Add("p1", NpgsqlDbType.Varchar, 32);
        p1.Value = val;
        var r = await cmd.ExecuteReaderAsync();
        int total = 0;
        while(r.Read()){
            Object[] values = new Object[r.FieldCount];
            int fieldCount = r.GetValues(values);
            College item = new College{
                Id = (string)values[0], Name = (string)values[1],City = (string)values[2],State = (string)values[3],
                Zip = (string)values[4],Region = (int)values[5], 
                Latitude = ToDecimal(values[6]), Longitude = ToDecimal(values[7]),
                AdmRate = ToDecimal(values[8]), SatAvg = ToDecimal(values[9]), ActAvg=ToDecimal(values[10]),
                Earnings=ToDecimal(values[11]), Cost=ToDecimal(values[12]),Enrollments=ToDecimal(values[13])
            };
            // Log.Information(item.Info);
            Console.WriteLine(item.Info);
            total ++;
        }
        ret = total;
    }
    return ret;
}

public async Task<List<College>>GetColleges(int Region, string ConnectionString){
    // int ret = 0;
    // var clsName = this.GetType().Name;
    // Log.Information($"{clsName}.Run .... ");
    var results = new List<College>();
    using(var cn = new NpgsqlConnection(ConnectionString)){
        string sqltext = "select * from yin.college where earnings is not null and region = @p1 order by name";
        await cn.OpenAsync();
        var cmd = new NpgsqlCommand(sqltext, cn);
        var p1 = cmd.Parameters.Add("p1", NpgsqlDbType.Integer);
        p1.Value = Region;
        var r = await cmd.ExecuteReaderAsync();
        int total = 0;
        while(r.Read()){
            Object[] values = new Object[r.FieldCount];
            int fieldCount = r.GetValues(values);
            College item = new College{
                Id = (string)values[0], Name = (string)values[1],City = (string)values[2],State = (string)values[3],
                Zip = (string)values[4],Region = (int)values[5], 
                Latitude = ToDecimal(values[6]), Longitude = ToDecimal(values[7]),
                AdmRate = ToDecimal(values[8]), SatAvg = ToDecimal(values[9]), ActAvg=ToDecimal(values[10]),
                Earnings=ToDecimal(values[11]), Cost=ToDecimal(values[12]),Enrollments=ToDecimal(values[13])
            };
            // Log.Information(item.Info);
            Console.WriteLine(item.Info);
            total ++;
            results.Add(item);
        }
        Console.WriteLine($"{total} colleges found.");
    }
    return results;
}
