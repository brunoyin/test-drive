
// #reference "System.Speech"

using System;
// using System.Speech;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Runtime;

// using Npgsql;
// using CsvHelper;

var target = Argument("target", "Default");

Setup(context =>
{
    // info
    var framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
    var dt = DateTime.Now;
    Information($"Current dotnet time = {dt}\n\n");
    object[] list = Assembly.GetEntryAssembly().GetCustomAttributes(true);
    var attribute = list.OfType<TargetFrameworkAttribute>().First();

    Information(attribute.FrameworkName);
    Information(attribute.FrameworkDisplayName);
});

Teardown(context =>
{
    // done
    Information($"It's done {DateTime.Now}\n");
});


Task("Default")
    .Does(() =>
{
    // Information($"Hello! {DateTime.Now}");
    // foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()){
    //     Information(asm.FullName);
    // }
    // var speak = new SpeechSynthesizer();
    // speak.Speak("good morning!");
    if (Bamboo.IsRunningOnBamboo){
        Information("Running on Bamboo");
    }else
    {
        Information("It's not on Bamboo");
    }
});

Task("Postgres")
    .IsDependentOn("Default")
    .Does(
        // async () => {
        //     Information("Running Postgres query");
        //     var connectionString = string.Format("Host={0};Database={1};Username={2};Password={3}",
        //         Environment.GetEnvironmentVariable("PGHOST") ?? "192.168.0.48",
        //         Environment.GetEnvironmentVariable("PGDATABASE") ?? "bruno",
        //         Environment.GetEnvironmentVariable("PGUSER") ?? "bruno",
        //         Environment.GetEnvironmentVariable("PGPASSWORD") ?? "bruno"
        //     );
        //     var total = await Run("Cali", true, connectionString);
        //     Information($"Total found: {total} collges");
        // }
        ()=>{
            Information($"Dummy task done {DateTime.Now}\n");
        }
    );

RunTarget(target);
