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
using Azure.Identity;
using System.IO;
using System.Reflection;

namespace ApiThreeLayerArch
{
    public class Program
    {
        // main entry point for your application
        public static void Main(string[] args)
        {
            /*var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if(environment == Environments.Development)
            {

            }*/

            //Read Configuration from appsetting.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Logs", "log.json");

            var telemetryConfiguration = TelemetryConfiguration
            .CreateDefault();

            //telemetryConfiguration.InstrumentationKey = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];

            telemetryConfiguration.InstrumentationKey =
                "ee3cf2a0-c20b-467c-8309-7a04ad32cd62";

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                //.WriteTo.Console(new RenderedCompactJsonFormatter()) 
                //.WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
                //.WriteTo.RollingFile(new CompactJsonFormatter(), "./logs/app-{Date}.json")
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
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
                //    config.AddAzureKeyVault(
                //    keyVaultEndpoint,
                //    new DefaultAzureCredential());
                //})
                .UseSerilog()   ////Uses Serilog instead of default .NET Logger
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
