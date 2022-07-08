using Microsoft.EntityFrameworkCore;
using Sgart.Net.ConsoleApp.BO;
using Sgart.Net.ConsoleApp.BO.DTO;
using Sgart.Net.ConsoleApp.BO.Entities;
using Sgart.Net.ConsoleApp.BO.InputDTO;
using Sgart.Net.ConsoleApp.Data;
using Sgart.Net.WebReactApp2.Models;

namespace Sgart.Net.WebReactApp2.Services
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
            _logger.LogDebug($"GetAllAsync starting...");
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
                _logger.LogError(ex, "GetAllAsync error");
            }
            return new List<TodoDTO>();
        }

        public async Task<string> GetExcel(System.IO.Stream strm)
        {
            string fileName = $"Sgart_todo_excel_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{SimpleExcelService.FILE_EXTENSION}";
            _logger.LogTrace($"Excel nema: {fileName}");

            var items = await GetAllAsync();

            _excelService.Create(strm);

            _excelService.AddSheet("Todo");

            _excelService.AddHeaders(new List<string> { "Id", "Messaggio", "Completato", "Modificato il" });

            if (items?.Count > 0)
            {
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
            }
            _excelService.Close();

            return fileName;
        }


        public async Task<TodoDTO?> GetAsync(int todoId)
        {
            _logger.LogDebug($"GetAsync {todoId} starting...");
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
                _logger.LogError(ex, "GetAsync error");
            }
            return null;

        }


        public async Task<Todo?> AddAsync(TodoAddDTO data)
        {
            _logger.LogDebug($"AddAsync starting...");
            _logger.LogTrace($"AddAsync: {System.Text.Json.JsonSerializer.Serialize(data)}");

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

                return d;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddAsync error");
                return null;
            }

        }

        public async Task<bool> EditAsync(TodoEditDTO data)
        {
            _logger.LogDebug($"EditAsync starting...");
            _logger.LogTrace($"EditAsync: {System.Text.Json.JsonSerializer.Serialize(data)}");

            try
            {
                var item = await _context.Todos.FirstOrDefaultAsync(x => x.TodoId == data.TodoId);

                if (item == null)
                {
                    _logger.LogWarning($"Todo id {data.TodoId} not found");
                }
                else
                {
                    // se non cambio l'oggetto non si accorge delle modifiche
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
                _logger.LogError(ex, "EditAsync error");
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int todoId)
        {
            _logger.LogDebug($"DeleteAsync {todoId} starting...");
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
                _logger.LogError(ex, "DeleteAsync error");
            }
            return false;
        }
    }
}
