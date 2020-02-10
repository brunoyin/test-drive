using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LdapAuthDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            // .AddJsonFile("certificate.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .Build();

            var hostUrl = config["hosturl"];
            if (string.IsNullOrEmpty(hostUrl)) hostUrl = "http://0.0.0.0:5000";

            CreateWebHostBuilder(args)
                .UseKestrel(
                    options =>
                    {
                        options.AddServerHeader = false;
                        options.ListenAnyIP(5000);
                    }
                )
                .UseConfiguration(config)
                .UseUrls(hostUrl)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>();
    }
}
