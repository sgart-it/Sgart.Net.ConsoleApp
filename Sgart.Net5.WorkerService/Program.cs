using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using NLog.Extensions.Logging;
using Sgart.Net5.ConsoleApp.Data;
using Sgart.Net5.WorkerService.Models;
using Sgart.Net5.WorkerService.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net5.WorkerService
{
    public class Program
    {
        static bool _isService = WindowsServiceHelpers.IsWindowsService();

        // ATTENZIONE nel caso di servizio windows, System.IO.Directory.GetCurrentDirectory() ritorna sempre C:\Windows\system32
        static string _currentDirectory = _isService ? AppDomain.CurrentDomain.BaseDirectory : System.IO.Directory.GetCurrentDirectory();

        public static async Task<int> Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(_currentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            NLog.LogManager.Configuration = new NLog.Extensions.Logging.NLogLoggingConfiguration(config.GetSection("NLog"));

            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                logger.Debug($"Sgart.Net.WorkerService - Start main in {_currentDirectory}");

                logger.Warn($"Runnig as {(_isService ? "SERVICE" : "EXE")}");

                await CreateHostBuilder(args).Build().RunAsync();

                return 0;   // OK
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped unknow exception");
                // scrivo anche in console nel caso il log non funzionasse
                // in modo da evidenziare eventuali errori non gestiti
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureAppConfiguration((context, config) =>
                //{
                //    // configure the app here.
                //})
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    // quando eseguito come servizo, services.AddSingleton genera exception
                    //services.AddSingleton(configuration.GetSection("Settings").Get<AppSettings>());

                    services.Configure<AppSettings>(configuration.GetSection("Settings"));

                    // registro EF DB context
                    services.AddDbContext<SgartDbContext>(options =>
                    {
                        options.UseSqlServer(
                            configuration.GetConnectionString("DefaultConnection"),
                            // imposto in quale assembly si trovano le migration
                            b => b.MigrationsAssembly(typeof(SgartDbContext).Assembly.GetName().Name)
                        );
                    }, optionsLifetime: ServiceLifetime.Singleton);

                    // registro il Worker
                    services.AddHostedService<SgartWorker>();
                })
                .ConfigureLogging(logBuilder =>
                {
                    logBuilder.SetMinimumLevel(LogLevel.Trace);
                    // aggiungo NLog
                    logBuilder.AddNLog();
                })
                .UseWindowsService(); // imposto come Windows Service
    }
}
