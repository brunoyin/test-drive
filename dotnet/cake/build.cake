// dotnet add package Npgsql --version 4.0.5
// #addin nuget:?package=Npgsql&version=4.0.5
// dotnet add package CsvHelper --version 12.1.2
// #addin nuget:?package=CsvHelper&version=12.1.2

#l query2csv.cake

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Runtime;

// using Npgsql;
// using CsvHelper;

var target = Argument("target", "Postgres");

Setup(context =>
{
    // info
    var framework = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;
    Information($"Current dotnet framework = {framework}\n\n");
    // object[] list = Assembly.GetEntryAssembly().GetCustomAttributes(true);
    // var attribute = list.OfType<TargetFrameworkAttribute>().First();

    // Information(attribute.FrameworkName);
    // Information(attribute.FrameworkDisplayName);
});

Teardown(context =>
{
    // done
    Information($"It's done {DateTime.Now}\n");
});

TaskSetup(setupContext =>
{
    var message = string.Format("Task: {0} at {1}", setupContext.Task.Name, DateTime.Now);
    Information($"Message in TaskSetup => {message}\n");
});

TaskTeardown(teardownContext =>
{
    var message = string.Format("Task: {0}", teardownContext.Task.Name);
    Information($"Message in TaskTeardown => {message}\n");
});

Task("Default")
    .Does(() =>
{
    // Information($"Hello! {DateTime.Now}");
    foreach(var asm in AppDomain.CurrentDomain.GetAssemblies()){
        Information(asm.FullName);
    }
});

Task("Postgres")
    .IsDependentOn("Default")
    .Does(
        async () => {
            Information("Running Postgres query");
            var connectionString = string.Format("Host={0};Database={1};Username={2};Password={3}",
                Environment.GetEnvironmentVariable("PGHOST") ?? "192.168.0.48",
                Environment.GetEnvironmentVariable("PGDATABASE") ?? "bruno",
                Environment.GetEnvironmentVariable("PGUSER") ?? "bruno",
                Environment.GetEnvironmentVariable("PGPASSWORD") ?? "bruno"
            );
            var total = await Run("Cali", true, connectionString);
            Information($"Total found: {total} collges");
        }
    );

RunTarget(target);
