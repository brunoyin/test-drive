﻿using System;
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
            var tmpDir = Environment.GetEnvironmentVariable("TEST_TMP_DIR") ?? Environment.CurrentDirectory;
            var logFile = Path.Combine(tmpDir, $"CmdlineApp-log-{dt:yyyyMMddHHmmss}.txt");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFile)
                .CreateLogger();
            Log.Information("Getting started");

            var app = new CommandLineApplication();
            app.HelpOption("-h|--help");
            var optionId = app.Option("-id|--college-id <ID>", "college id in college scorecard db", CommandOptionType.SingleValue);
            var optionName = app.Option("-n|--name <NAME>", "college name", CommandOptionType.SingleValue);
            var optionQueryType = app.Option("-qt|--query-type <QUERYTYPE>", "EF or Plain old ADO.Net", CommandOptionType.SingleValue);
            var optionCsv = app.Option("-csv|--export-csv <CSVFILENAME>", "Export to a csv file", CommandOptionType.SingleValue);

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
                if (optionCsv.HasValue()){
                    var exporter = new CsvExporter(connectionString);
                    exporter.WriteToCsv("select * from yin.college limit 500", optionCsv.Value());
                }else{
                    bool useEF = optionQueryType.HasValue() && optionQueryType.Value() == "ef";
                    IPostgresQuery q;
                    if(useEF) q = new EFQuery(); else q = new SimpleQuery();
                    q.ConnectionString = connectionString;
                    await q.Run(optionName.HasValue()?optionName.Value() : collegeId, optionName.HasValue());
                }
            });

            return app.Execute(args);
        }
    }
}
