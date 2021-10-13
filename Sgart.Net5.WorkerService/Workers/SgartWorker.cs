using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sgart.Net5.ConsoleApp.Data;
using Sgart.Net5.WorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sgart.Net5.WorkerService.Workers
{
    public class SgartWorker : BackgroundService
    {
        private readonly ILogger<SgartWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly AppSettings _settings;
        private readonly int _workerPauseMilliseconds;

        // non posso fare l'inject del db context su un Worker, devo passare per IServiceScopeFactory, vedi ExecuteAsync
        private SgartDbContext _context;

        public SgartWorker(ILogger<SgartWorker> logger, IServiceScopeFactory serviceScopeFactory, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _settings = settings.Value;

            // imposto un minimo di 1secondo di pausa
            _workerPauseMilliseconds = (_settings.WorkerPauseSeconds < 1 ? 1 : _settings.WorkerPauseSeconds) * 1000;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace("ExecuteAsync");
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        _context = scope.ServiceProvider.GetRequiredService<SgartDbContext>();

                        await Process();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ProcessdNotifications error");
                }

                _logger.LogTrace("Pause {time} ms", _workerPauseMilliseconds);
                await Task.Delay(_workerPauseMilliseconds, stoppingToken); // pausa 10 secondi
            }

        }

        public async Task Process()
        {
            // .AsNoTracking() aumenta la velocità delle query di sola lettura
            var items = await _context.Todos.AsNoTracking()
                .OrderBy(x => x.TodoId)
                .Take(2)    // prendo solo 2 record per demo
                .ToListAsync();

            foreach (var item in items)
            {
                _logger.LogInformation($"{item.TodoId}) {item.DataJson.Text} {(item.DataJson.Completed == true ? "[completed]" : "")}");
            }
        }
    }
}
