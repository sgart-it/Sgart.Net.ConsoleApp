using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net5.WebReactApp.Services
{
    /// <summary>
    /// Crea un file excel base per l'esport dei dati in forma tabellare
    /// si può aggiungere un header e supporta date e numeri
    /// richiede il pacchetto NuGet DocumentFormat.OpenXml Version="2.15.0"
    /// </summary>
    public class SimpleExcelService : IDisposable
    {
        private readonly ILogger<SimpleExcelService> _logger;
        private readonly System.Globalization.CultureInfo _ciEN;

        public SimpleExcelService(ILogger<SimpleExcelService> logger)
        {
            _logger = logger;
            _logger.LogTrace("Export Excel");
            _ciEN = new System.Globalization.CultureInfo("en-US");

        }

        public const string CONTENT_TYPE = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string FILE_EXTENSION = "xlsx";
        private const string CELL_BASE_NAMES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly SpreadsheetDocument _package;
        private SheetData _sheetData;
        private Row _row;
        private int _rowNumber = 1;
        private int _cellNumber = 1;

        private int _styleFormatDate = 0;
        private int _styleFormatDateTime = 0;

        public SimpleExcelService(string fileName)
        {
            _package = SpreadsheetDocument.Create(fileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            Init();
        }

        public SimpleExcelService(System.IO.Stream stream)
        {
            _package = SpreadsheetDocument.Create(stream, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook);
            Init();
        }

        private void Init()
        {
            var workbookPart = _package.AddWorkbookPart();
            workbookPart.Workbook = new Workbook
            {
                Sheets = new Sheets()
            };

            // Add Stylesheet.
            var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
            var stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = GetStylesheet();
            workbookStylesPart.Stylesheet.Save();
        }

        private Stylesheet GetStylesheet()
        {
            //https://stackoverflow.com/questions/7089745/openxml-writing-a-date-into-excel-spreadsheet-results-in-unreadable-content/31829959#31829959
            var styleSheet = new Stylesheet();

            // Create "fonts" node.
            var fonts = new Fonts();
            fonts.Append(new Font()
            {
                FontName = new FontName() { Val = "Calibri" },
                FontSize = new FontSize() { Val = 11 },
                FontFamilyNumbering = new FontFamilyNumbering() { Val = 2 },
            });

            fonts.Count = (uint)fonts.ChildElements.Count;

            // Create "fills" node.
            var fills = new Fills();
            fills.Append(new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.None }
            });
            fills.Append(new Fill()
            {
                PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 }
            });

            fills.Count = (uint)fills.ChildElements.Count;

            // Create "borders" node.
            var borders = new Borders();
            borders.Append(new Border()
            {
                LeftBorder = new LeftBorder(),
                RightBorder = new RightBorder(),
                TopBorder = new TopBorder(),
                BottomBorder = new BottomBorder(),
                DiagonalBorder = new DiagonalBorder()
            });
            borders.Count = (uint)borders.ChildElements.Count;

            // Create "cellStyleXfs" node.
            var cellStyleFormats = new CellStyleFormats();
            cellStyleFormats.Append(new CellFormat()
            {
                NumberFormatId = 0,
                FontId = 0,
                FillId = 0,
                BorderId = 0
            });

            cellStyleFormats.Count = (uint)cellStyleFormats.ChildElements.Count;

            // Create "cellXfs" node.
            var cellFormats = new CellFormats();

            // A default style that works for everything but DateTime
            cellFormats.Append(new CellFormat()
            {
                BorderId = 0,
                FillId = 0,
                FontId = 0,
                NumberFormatId = 0,
                FormatId = 0,
                ApplyNumberFormat = true
            });

            // Date only
            cellFormats.Append(new CellFormat()
            {
                BorderId = 0,
                FillId = 0,
                FontId = 0,
                NumberFormatId = 14,    // Date only
                FormatId = 0,
                ApplyNumberFormat = true
            });

            cellFormats.Count = (uint)cellFormats.ChildElements.Count;

            _styleFormatDate = cellFormats.ChildElements.Count - 1;

            // Date + Time
            cellFormats.Append(new CellFormat()
            {
                BorderId = 0,
                FillId = 0,
                FontId = 0,
                NumberFormatId = 22,    // Date + Time
                FormatId = 0,
                ApplyNumberFormat = true
            });

            cellFormats.Count = (uint)cellFormats.ChildElements.Count;

            _styleFormatDateTime = cellFormats.ChildElements.Count - 1;

            // Create "cellStyles" node.
            var cellStyles = new CellStyles();
            cellStyles.Append(new CellStyle()
            {
                Name = "Normal",
                FormatId = 0,
                BuiltinId = 0
            });
            cellStyles.Count = (uint)cellStyles.ChildElements.Count;

            // aggiungo tutti nodi in ordine
            styleSheet.Append(fonts);
            styleSheet.Append(fills);
            styleSheet.Append(borders);
            styleSheet.Append(cellStyleFormats);
            styleSheet.Append(cellFormats);
            styleSheet.Append(cellStyles);

            return styleSheet;
        }

        /// <summary>
        /// ritorna il numero di riga corrente
        /// </summary>
        /// <returns></returns>
        public int GetRowNumber()
        {
            return _rowNumber;
        }

        /// <summary>
        /// aggiunge una scheda all'Excel
        /// da richiamare come prima operazione dopo il costruttore
        /// </summary>
        /// <param name="sheetName"></param>
        public void AddSheet(string sheetName)
        {
            var sheetPart = _package.WorkbookPart.AddNewPart<WorksheetPart>();
            _sheetData = new SheetData();
            sheetPart.Worksheet = new Worksheet(_sheetData);

            var sheets = _package.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            string relationshipId = _package.WorkbookPart.GetIdOfPart(sheetPart);

            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
            }

            var sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
        }

        /// <summary>
        /// aggiunta di header al Sheet (opzionale)
        /// </summary>
        /// <param name="headers"></param>
        public void AddHeaders(List<string> headers)
        {
            NewRow();
            foreach (var title in headers)
            {
                AddCell(title);
            }
            AddRow();
        }

        /// <summary>
        /// Crea una nuova riga ma NON la aggiunge al Sheet
        /// </summary>
        /// <returns></returns>
        public int NewRow()
        {
            _row = new Row();
            return _rowNumber;
        }

        /// <summary>
        /// aggiunge una nuova riga al Sheet
        /// </summary>
        public void AddRow()
        {
            _sheetData.AppendChild(_row);
            _row = null;
            _rowNumber++;
            _cellNumber = 1;
        }

        private static Cell NewCell(CellValues type)
        {
            var cell = new Cell
            {
                DataType = type
            };
            return cell;
        }

        private string AddrowInternal(Cell cell)
        {
            var cn = _cellNumber - 1; // -1 perchè parte da 1 e non 0

            _row.AppendChild(cell);
            _cellNumber++;

            if (cn > 26)
            {
                var I1 = cn / 26;
                var I0 = cn % 26;

                return CELL_BASE_NAMES[I0].ToString() + CELL_BASE_NAMES[I1].ToString();
            }
            return CELL_BASE_NAMES[cn].ToString();
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo Stringa
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(string value)
        {
            var cell = NewCell(CellValues.String);
            cell.CellValue = new CellValue(value);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo Formula
        /// </summary>
        /// <param name="value"></param>
        public string AddCellFormula(string formula)
        {
            var cell = NewCell(CellValues.String);
            cell.CellFormula = new CellFormula(formula);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo boleano
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(bool value)
        {
            var cell = NewCell(CellValues.Boolean);
            cell.CellValue = new CellValue(value);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo Intero
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(int value)
        {
            var cell = NewCell(CellValues.Number);
            cell.CellValue = new CellValue(value);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo Decimal
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(decimal value)
        {
            var cell = NewCell(CellValues.Number);
            cell.CellValue = new CellValue(value);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo Decimal nullable
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(decimal? value)
        {
            var cell = NewCell(CellValues.Number);
            if (value.HasValue)
            {
                cell.CellValue = new CellValue(value.Value);
            }
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo DateTime
        /// TODO. non funzionna bene da sistemare
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(DateTime value, bool showTime = false)
        {
            var cell = NewCell(CellValues.Date);
            double oaValue = value.ToOADate();
            cell.CellValue = new CellValue(oaValue.ToString(_ciEN));
            cell.StyleIndex = Convert.ToUInt32(showTime ? _styleFormatDateTime : _styleFormatDate);
            return AddrowInternal(cell);
        }

        /// <summary>
        /// aggiunge, ad una riga, una cella di tipo DateTime nullable
        /// </summary>
        /// <param name="value"></param>
        public string AddCell(DateTime? value, bool showTime = false)
        {
            var cell = NewCell(CellValues.Date);
            if (value.HasValue)
            {
                double oaValue = value.Value.ToOADate();
                cell.CellValue = new CellValue(oaValue.ToString(_ciEN));
            }
            cell.StyleIndex = Convert.ToUInt32(showTime ? _styleFormatDateTime : _styleFormatDate);
            return AddrowInternal(cell);
        }

        public void Dispose()
        {
            if (_package != null)
                _package.Dispose();
        }


    }
}