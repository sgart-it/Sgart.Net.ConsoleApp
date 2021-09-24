using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sgart.Net5.ConsoleApp.BO;
using Sgart.Net5.ConsoleApp.BO.DTO;
using Sgart.Net5.ConsoleApp.BO.InputDTO;
using Sgart.Net5.ConsoleApp.Data;
using Sgart.Net5.WebReactApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net5.WebReactApp.Services
{
    public class SgartDIExampleService
    {
        private readonly ILogger<SgartDIExampleService> _logger;
        private readonly SgartDbContext _context;
        private readonly AppSettings _settings;

        public SgartDIExampleService(ILogger<SgartDIExampleService> logger, SgartDbContext context, AppSettings settings)
        {
            _logger = logger;
            _context = context;
            _settings = settings;
        }
        
        /* implementare i metodi necessari */

        public async Task<List<TodoDTO>> GetTodoAllAsync()
        {
            _logger.LogDebug($"GetTodosAsync starting...");

            // .AsNoTracking() aumenta la velocità delle query di sola lettura
            return await _context.Todos.AsNoTracking()
                .OrderBy(x => x.TodoId)
                .Select(item => new TodoDTO
                {
                    TodoId = item.TodoId,
                    Message = item.DataJson.Text,
                    Completed = item.DataJson.Completed
                }).ToListAsync();

            //var items = await _context.Todos.AsNoTracking()
            //    .OrderBy(x => x.TodoId)
            //    .ToListAsync();
            //var result = new List<TodoDTO>();
            //foreach (var item in items)
            //{
            //    result.Add(new TodoDTO
            //    {
            //        TodoId = item.TodoId,
            //        Message = item.DataJson.Text,
            //        Completed = item.DataJson.Completed
            //    });
            //}
            //return result;
        }

        public async Task AddTodoAsync(TodoAddDTO data)
        {
            _logger.LogDebug($"AddTodosAsync starting...");

            var d = new Todo
            {
                TodoId = 0,
                DataJson = new TodoData
                {
                    Text = data.Message,
                    Completed = data.Completed
                },
                Created = DateTime.Now
            };

            await _context.Todos.AddAsync(d);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> EditTodoAsync(TodoEditDTO data)
        {
            _logger.LogDebug($"EditTodosAsync starting...");

            var item = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == data.TodoId);

            if(item == null)
            {
                _logger.LogError($"Todo id {data.TodoId} not found");
                return false;
            }
            item.DataJson.Text = data.Message;
            item.DataJson.Completed = data.Completed;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTodoAsync(int todoId)
        {
            _logger.LogDebug($"EditTodosAsync starting...");

            var item = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == todoId);

            if (item == null)
            {
                _logger.LogError($"Todo id {todoId} not found");
                return false;
            }
            _context.Remove(item);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
