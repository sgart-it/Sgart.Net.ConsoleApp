using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using System.Threading.Tasks;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Sgart.Net5.ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Sgart.Net5.ConsoleApp.Data;
using Sgart.Net5.ConsoleApp.Services;

namespace Sgart.Net5.ConsoleApp
{
    /// <summary>
    /// demo console application in .Net 5
    /// con Entity Framework e NLog
    /// </summary>
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            NLog.LogManager.Configuration = new NLog.Extensions.Logging.NLogLoggingConfiguration(config.GetSection("NLog"));

            var logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                logger.Debug("Sgart.Net.ConsoleApp - Start main");

                // istanzio l'host
                return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                    .MapResult(async (opts) =>
                    {
                        var host = CreateHostBuilder(args, opts).Build();

                        using (var serviceScope = host.Services.CreateScope())
                        {
                            // applicazione migration e dati iniziali al database
                            DbUpdater.Upgrade(serviceScope);

                            var services = serviceScope.ServiceProvider;

                            // creo la app
                            var runner = services.GetRequiredService<SgartConsoleService>();
                            await runner.Run();
                        }

                        return 0;
                    },
                    errs => Task.FromResult(-1)); // Invalid arguments

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped unknow exception");
                return -1;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args, CommandLineOptions opts) =>
            Host.CreateDefaultBuilder(args)
                //.ConfigureLogging(configureLogging => configureLogging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information))
                .ConfigureServices((hostContext, services) =>
                {

                    // specifico qual'è il file con la configurazione di NLog
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                       .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                       .Build();

                    services.AddSingleton(configuration.GetSection("Settings").Get<AppSettings>());

                    services.AddDbContext<SgartDbContext>(options =>
                    {
                        options.UseSqlServer(
                            configuration.GetConnectionString("DefaultConnection"),
                            // imposto in quale assembly si trovano le migration
                            b => b.MigrationsAssembly(typeof(SgartDbContext).Assembly.GetName().Name)
                            //b => b.MigrationsAssembly("Sgart.Net5.ConsoleApp")
                        );
                    });

                    // aggiungo la configurazione di NLog
                    NLog.LogManager.Configuration = new NLog.Extensions.Logging.NLogLoggingConfiguration(configuration.GetSection("NLog"));
                    services.AddSingleton<IConfiguration>(configuration);

                    services.AddSingleton(opts);

                    // registro la classe con il codice della console app
                    services.AddTransient<SgartConsoleService>();
                })
            .ConfigureLogging(logBuilder =>
            {
                logBuilder.SetMinimumLevel(LogLevel.Trace);
                // aggiungo NLog
                logBuilder.AddNLog();
            })
            .UseConsoleLifetime();
    }

}