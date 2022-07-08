using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sgart.Net.WebReactApp2.Models
{
    public class ExcelMockModel
    {

        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
    }
}
