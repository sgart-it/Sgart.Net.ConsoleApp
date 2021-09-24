using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net5.ConsoleApp.BO.InputDTO
{
    public class TodoEditDTO
    {
        public int TodoId { get; set; }
        public string Message { get; set; }
        public bool Completed { get; set; }
    }
}
