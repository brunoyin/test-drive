using System;
using System.Threading.Tasks;

using McMaster.Extensions.CommandLineUtils;

using System.IO;
using System.Collections.Generic;

using Serilog;

namespace CmdlineApp
{
    class Program
    {
        public static string connectionString = "";
        static int Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            var dt = DateTime.Now;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory, $"log-{dt:yyyyMMddHHmmss}.txt"))
                .CreateLogger();
            Log.Information("Getting started");

            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");
            var optionId = app.Option("-id|--college-id <ID>", "college id in college scorecard db", CommandOptionType.SingleValue);
            var optionName = app.Option("-n|--name <NAME>", "college name", CommandOptionType.SingleValue);
            var optionQueryType = app.Option("-qt|--query-type <QUERYTYPE>", "EF or Plain old ADO.Net", CommandOptionType.SingleValue);
            string collegeId = optionId.HasValue()?optionId.Value(): "186380";

            connectionString = string.Format("Host={0};Database={1};Username={2};Password={3}",
                Environment.GetEnvironmentVariable("PGHOST") ?? "192.168.0.48",
                Environment.GetEnvironmentVariable("PGDATABASE") ?? "bruno",
                Environment.GetEnvironmentVariable("PGUSER") ?? "bruno",
                Environment.GetEnvironmentVariable("PGPASSWORD") ?? "bruno"
            );

            Console.WriteLine(connectionString);

            app.OnExecute(async ()=>{
                await Task.Delay(200);
                // Console.WriteLine("Hello World!");
                bool useEF = optionQueryType.HasValue() && optionQueryType.Value() == "ef";
                IPostgresQuery q;
                if(useEF) q = new EFQuery(); else q = new SimpleQuery();
                q.ConnectionString = connectionString;
                await q.Run(optionName.HasValue()?optionName.Value() : collegeId, optionName.HasValue());
            });

            return app.Execute(args);
        }
    }
}
