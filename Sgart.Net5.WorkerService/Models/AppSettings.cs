using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sgart.Net5.WorkerService.Models
{
    /// <summary>
    /// classe di mapping delle proprietà presenti in appsettings.json\Settings
    /// </summary>
    public class AppSettings
    {
        public int WorkerPauseSeconds { get; set; }
    }
}
