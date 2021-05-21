using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiThreeLayerArch
{
    public class Program
    {
        // main entry point for your application
        public static void Main(string[] args)
        {
            //Read Configuration from appsetting.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var telemetryConfiguration = TelemetryConfiguration
            .CreateDefault();
            telemetryConfiguration.InstrumentationKey =
                "5ba9683a-1a52-409a-82c7-0012401a70e5";

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)   //.WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
                .CreateLogger();

            try
            {
                Log.Information("TSM Starting web host");
                // create the host builder
                CreateHostBuilder(args)
                    // build the host
                    .Build()
                    // and run the host, i.e. your web application
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "TSM Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()   ////Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
