using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using LogLevel = NLog.LogLevel;

namespace CodingMilitia.PlayBall.GroupManagement.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("############### Starting application ###############");
            ConfigureNLog();
            CreateWebHostBuilder(args).Build().Run();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.ClearProviders(); //clear default providers, NLog will handle it
                    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); //set minimum level to trace, NLog rules will kick in afterwards
                })
                .UseNLog()
                .UseStartup<Startup>();
        
        //TODO: replace with nlog.config
        private static void ConfigureNLog()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget("coloredConsole")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            
            var fileTarget = new FileTarget("file")
            {
                FileName = "${basedir}/file.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception} ${ndlc}"
            };
            config.AddTarget(fileTarget);
            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget, "CodingMilitia.PlayBall.GroupManagement.Web.IoC.*");
            config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
            config.AddRule(LogLevel.Warn, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }
    }
}
