using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sgart.Net5.WebReactApp.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sgart.Net5.WebReactApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {

        private readonly ILogger<TodoController> _logger;
        private readonly ExportService _service;

        public ExportController(ILogger<TodoController> logger, ExportService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("excel")]
        public async Task<IActionResult> Excel()
        {
            try
            {
                var mem = new MemoryStream();

                string fileName = await _service.GetExcel(mem);

                mem.Position = 0;

                return new FileStreamResult(mem, SimpleExcelService.CONTENT_TYPE)
                {
                    FileDownloadName = fileName,
                    LastModified = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel");

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("excel2007")]
        public async Task<IActionResult> Excel2007()
        {
            try
            {
                var mem = new MemoryStream();

                string fileName = await _service.GetExcel(mem, true);

                mem.Position = 0;

                return new FileStreamResult(mem, SimpleExcelService.CONTENT_TYPE)
                {
                    FileDownloadName = fileName,
                    LastModified = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel");

                return BadRequest();
            }
        }


        /// <summary>
        /// demo salvataggio in locale
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult<bool>> ExcelSaveLocaly()
        {
            try
            {
                // using (var xls = new ExcelSimpleExport(@"c:\temp\provaX2.xlsx"))
                using (MemoryStream mem = new())
                {
                    string fileName = await _service.GetExcel(mem);

                    mem.Position = 0;

                    using FileStream file = new(@$"c:\temp\{fileName}", FileMode.OpenOrCreate, FileAccess.Write);
                    mem.CopyTo(file);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExcelSaveLocaly");

                return BadRequest();
            }
        }
    }
}
