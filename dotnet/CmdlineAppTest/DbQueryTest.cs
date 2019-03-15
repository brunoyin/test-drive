using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using CmdlineApp;
using Serilog;

namespace CmdlineAppTest
{
    public class DbQueryTest
    {
        ITestOutputHelper output;
        string connectionString;
        public DbQueryTest(ITestOutputHelper output){
            this.output = output;
            var clsName = this.GetType().Name;
            var tmpDir = Environment.GetEnvironmentVariable("TEST_TMP_DIR") ?? Environment.CurrentDirectory;
            var testLogFile = Path.Combine(tmpDir, $"{clsName}-log-{DateTime.Now:yyyyMMddHHmmss}.txt");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(testLogFile)
                .CreateLogger();
            connectionString = string.Format("Host={0};Database={1};Username={2};Password={3}",
                Environment.GetEnvironmentVariable("PGHOST") ?? "192.168.0.48",
                Environment.GetEnvironmentVariable("PGDATABASE") ?? "bruno",
                Environment.GetEnvironmentVariable("PGUSER") ?? "bruno",
                Environment.GetEnvironmentVariable("PGPASSWORD") ?? "bruno"
            );
            Log.Information("started");
        }
        
        [Theory, MemberData(nameof(QueryData))]
        public async Task PostgresQueryTested(IPostgresQuery query, string name, string id)
        {
            var dt = DateTime.Now;
            query.ConnectionString = connectionString;
            var clsName = query.GetType().Name;
            Log.Information($"Running {clsName}.Run({name}, true)");
            var ret = await query.Run(name, true);
            Assert.True(ret > 0);

            Log.Information($"Running {clsName}.Run({id})");
            ret = await query.Run(id);
            Assert.True(ret == 1);
        }

        public static IEnumerable<object[]>QueryData{
            get {
                return new []{
                    new object[]{new SimpleQuery(), "tex", "186380" },
                    new object[]{new EFQuery(), "tex", "186380" }
                };
            }
        }
    }
}
