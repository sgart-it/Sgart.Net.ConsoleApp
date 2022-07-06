using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sgart.Net.ConsoleApp.Data;
using Sgart.Net.ConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net.ConsoleApp.Services
{
    public class SgartConsoleService
    {
        private readonly ILogger<SgartConsoleService> _logger;
        private readonly SgartDbContext _context;
        private readonly AppSettings _settings;

        public SgartConsoleService(ILogger<SgartConsoleService> logger, SgartDbContext context, AppSettings settings)
        {
            _logger = logger;
            _context = context;
            _settings = settings;
        }

        public async Task Run()
        {
            _logger.LogDebug($"Run starting...");

            // .AsNoTracking() aumenta la velocità delle query di sola lettura
            var items = await _context.Todos.AsNoTracking()
                .OrderBy(x => x.TodoId)
                .ToListAsync();

            foreach (var item in items)
            {
                Console.WriteLine($"{item.TodoId}) {item.DataJson.Text} {(item.DataJson.Completed ==true ? "[completed]" : "")}");
            }
            _logger.LogDebug($"Run end");
        }
    }
}
