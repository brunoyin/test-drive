using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

using Serilog;
using Microsoft.EntityFrameworkCore;

namespace CmdlineApp
{
    public class EFQuery : PostgresQuery{
        override public async Task<int> Run(string val, bool queryByName){
            string clsName = this.GetType().Name;
            Log.Information($"{clsName}.Run .... ");
            int ret = 0;
            brunoContext.ConnectionString = ConnectionString;
            string sqltext = (!queryByName) ? "select * from yin.college where id = {0}"
                :"select * from yin.college where earnings is not null and name ~* {0} order by earnings desc limit 20";
            using (var context = new brunoContext())
            {
                Task<List<College>> task = context.College
                    .FromSql(sqltext, val)
                    .ToListAsync();
                Console.WriteLine("Waiting for query to finish ...");
                var college = await task;
                foreach(var item in college){
                    Console.WriteLine(item.Info);
                    Log.Information(item.Info);
                }
                ret = college.Count;
            }
            return ret;
        }
    }
}