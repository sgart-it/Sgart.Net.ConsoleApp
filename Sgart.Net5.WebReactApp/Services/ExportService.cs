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
        private readonly SimpleExcelService _excelService;

        public ExportService(ILogger<ExportService> logger, SimpleExcelService excelService)
        {
            _logger = logger;
            _excelService = excelService;

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

        public async Task<string> GetExcel(Stream strm, bool dateformat2007 = false)
        {
            string fileName = $"Sgart_demo_export_excel_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.{SimpleExcelService.FILE_EXTENSION}";
            _logger.LogTrace($"Excel nema: {fileName}");

            var items = await GetMockData();

            _excelService.Create(strm);

            _excelService.Dateformat2007 = dateformat2007;

            _excelService.AddSheet("Sheet 1");

            _excelService.AddHeaders(new List<string> { "Id", "Title", "Value", "Date", "Value x 10" });

            foreach (var item in items)
            {
                // creo la riga 
                var rowNumber = _excelService.NewRow();

                // aggiungo i volori delle celle
                _excelService.AddCell(item.ID);
                _excelService.AddCell(item.Title);
                var lettValue = _excelService.AddCell(item.Value);
                _excelService.AddCell(item.Date);
                _excelService.AddCellFormula($"{lettValue}{rowNumber}*10");

                // aggiungo la riga all'excel
                _excelService.AddRow();
            }

            _excelService.Close();

            return fileName;
        }
    }
}
