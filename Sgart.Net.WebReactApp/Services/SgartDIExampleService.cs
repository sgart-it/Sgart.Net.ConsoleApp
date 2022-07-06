using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sgart.Net.ConsoleApp.BO;
using Sgart.Net.ConsoleApp.BO.DTO;
using Sgart.Net.ConsoleApp.BO.Entities;
using Sgart.Net.ConsoleApp.BO.InputDTO;
using Sgart.Net.ConsoleApp.Data;
using Sgart.Net.WebReactApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net.WebReactApp.Services
{
    public class SgartDIExampleService
    {
        private readonly ILogger<SgartDIExampleService> _logger;
        private readonly SgartDbContext _context;
        private readonly AppSettings _settings;
        private readonly SimpleExcelService _excelService;
        public SgartDIExampleService(ILogger<SgartDIExampleService> logger, SgartDbContext context, AppSettings settings, SimpleExcelService excelService)
        {
            _logger = logger;
            _context = context;
            _settings = settings;
            _excelService = excelService;
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

        public async Task<string> GetExcel(System.IO.Stream strm)
        {
            string fileName = $"Sgart_todo_excel_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{SimpleExcelService.FILE_EXTENSION}";
            _logger.LogTrace($"Excel nema: {fileName}");

            var items = await GetAllAsync();

            _excelService.Create(strm);

            _excelService.AddSheet("Todo");

            _excelService.AddHeaders(new List<string> { "Id", "Messaggio", "Completato", "Modificato il" });

            foreach (var item in items)
            {
                // creo la riga 
                _excelService.NewRow();

                // aggiungo i volori delle celle
                _excelService.AddCell(item.TodoId);
                _excelService.AddCell(item.Message);
                _excelService.AddCell(item.Completed);
                _excelService.AddCell(item.Modified, true);

                // aggiungo la riga all'excel
                _excelService.AddRow();

            }
            _excelService.Close();

            return fileName;
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
