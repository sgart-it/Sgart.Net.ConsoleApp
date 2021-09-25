using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog.Web;    // NLog

namespace Sgart.Net5.WebReactApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // configurazione NLog su appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            NLog.LogManager.Configuration = new NLog.Extensions.Logging.NLogLoggingConfiguration(config.GetSection("NLog"));
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(NLog.LogManager.Configuration).GetCurrentClassLogger();

            try
            {
                // scrivo nel log
                logger.Debug("Start main");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // rilascio le risorse usate da NLog
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                // configurazione NLog
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                // configurazione dependency injection NLog
                .UseNLog();
    }
}
