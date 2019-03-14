using System;
using System.Threading.Tasks;
using System.IO;

using Npgsql;
using NpgsqlTypes;

using Serilog;

namespace CmdlineApp
{
    public class SimpleQuery : PostgresQuery{
        
        decimal? ToDecimal(Object v){
            return (v is System.DBNull)? null: (decimal?)v;
        }

        override
        public async Task<int>Run(string val, bool queryByName){
            int ret = 0;
            var clsName = this.GetType().Name;
            Log.Information($"{clsName}.Run .... ");
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
                    Log.Information(item.Info);
                    Console.WriteLine(item.Info);
                    total ++;
                }
                ret = total;
            }
            return ret;
        }
    }
}