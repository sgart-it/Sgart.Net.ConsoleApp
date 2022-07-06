using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net.ConsoleApp.BO
{
    /// <summary>
    /// classe che verrà serializzata in un campo nvarchar su db
    /// vedi TodoConfiguration.cs
    /// </summary>
    public class TodoData
    {
        public string Text { get; set; }
        public bool Completed { get; set; }
    }
}
