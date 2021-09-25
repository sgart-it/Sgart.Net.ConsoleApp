using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sgart.Net5.ConsoleApp.BO;
using Sgart.Net5.ConsoleApp.BO.DTO;
using Sgart.Net5.ConsoleApp.BO.Entities;
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

        public async Task<List<TodoDTO>> GetAllAsync()
        {
            _logger.LogDebug($"GetTodosAsync starting...");
            try
            {
                // .AsNoTracking() aumenta la velocità delle query di sola lettura
                return await _context.Todos.AsNoTracking()
                    .OrderBy(x => x.TodoId)
                    .Select(item => new TodoDTO
                    {
                        TodoId = item.TodoId,
                        Message = item.DataJson.Text,
                        Completed = item.DataJson.Completed,
                        Modified = item.ModifiedUTC
                    }).ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddTodosAsync error");
            }
            return null;

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

        public async Task<TodoDTO> Get(int todoId)
        {
            _logger.LogDebug($"GetById starting...");
            try
            {
                // SingleOrDefaultAsync ritorna un singolo elemento oppure null se non trovato
                var item = await _context.Todos.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.TodoId == todoId);

                if (item == null)
                {
                    _logger.LogWarning($"Todo id {todoId} not found");
                }
                else
                {
                    return new TodoDTO
                    {
                        TodoId = item.TodoId,
                        Message = item.DataJson.Text,
                        Completed = item.DataJson.Completed,
                        Modified = item.ModifiedUTC
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddTodosAsync error");
            }
            return null;

        }


        public async Task AddAsync(TodoAddDTO data)
        {
            _logger.LogDebug($"AddTodosAsync starting...");
            try
            {
                var d = new Todo
                {
                    TodoId = 0,
                    DataJson = new TodoData
                    {
                        Text = data.Message,
                        Completed = data.Completed
                    },
                    CreatedUTC = DateTime.UtcNow
                };

                await _context.Todos.AddAsync(d);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddTodosAsync error");
            }

        }

        public async Task<bool> EditAsync(TodoEditDTO data)
        {
            _logger.LogDebug($"EditTodosAsync starting...");
            try
            {
                // TODO: ????
                var item = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == data.TodoId);

                if (item == null)
                {
                    _logger.LogWarning($"Todo id {data.TodoId} not found");
                }
                else
                {
                    // se non cambi l'oggetto non si accorge delle modifiche
                    item.DataJson = new TodoData
                    {
                        Text = data.Message,
                        Completed = data.Completed
                    };
                    item.ModifiedUTC = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EditTodosAsync error");
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int todoId)
        {
            _logger.LogDebug($"EditTodosAsync starting...");
            try
            {
                var item = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == todoId);

                if (item == null)
                {
                    _logger.LogError($"Todo id {todoId} not found");
                }
                else
                {
                    _context.Remove(item);

                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EditTodosAsync error");
            }
            return false;
        }
    }
}
