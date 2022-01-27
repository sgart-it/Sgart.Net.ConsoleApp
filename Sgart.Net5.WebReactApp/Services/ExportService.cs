using Microsoft.Extensions.Logging;
using Sgart.Net5.WebReactApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net5.WebReactApp.Services
{
    public class ExportService
    {
        private readonly ILogger<ExportService> _logger;
        private readonly SimpleExcelService _service;

        public ExportService(ILogger<ExportService> logger, SimpleExcelService service)
        {
            _logger = logger;
            _service = service;

            _logger.LogTrace("Export service");
        }

        private async Task<List<ExcelMockModel>> GetMockData()
        {
            var items = new List<ExcelMockModel> {
                new ExcelMockModel { ID=1, Title="Prova 1", Value=1231331.422M, Date= DateTime.Now },
                new ExcelMockModel { ID=2, Title="Prova 2", Value=731.22M, Date= new DateTime(2021,12,31) },
                new ExcelMockModel { ID=3, Title="Prova 3", Value=731.22M, Date= new DateTime(2021,5,21) }
            };

            return items;
        }

        public async Task<string> GetExcel(Stream strm)
        {
            string fileName = $"Sgart_demo_export_excel_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{SimpleExcelService.FILE_EXTENSION}";
            _logger.LogTrace($"Excel nema: {fileName}");

            var items = await GetMockData();

            using (var xls = new SimpleExcelService(strm))
            {
                xls.AddSheet("Sheet 1");

                xls.AddHeaders(new List<string> { "Id", "Title", "Value", "Date", "Value x 10" });

                foreach (var item in items)
                {
                    // creo la riga 
                    var rowNumber = xls.NewRow();

                    // aggiungo i volori delle celle
                    xls.AddCell(item.ID);
                    xls.AddCell(item.Title);
                    var lettValue = xls.AddCell(item.Value);
                    xls.AddCell(item.Date);
                    xls.AddCellFormula($"{lettValue}{rowNumber}*10");

                    // aggiungo la riga all'excel
                    xls.AddRow();
                }
            }
            return fileName;
        }
    }
}
