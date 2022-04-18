using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwaitPrc.Model
{
    public class ProgressReportModel
    {
        public List<string> ProgressStatus { get; set; } = new();
        public int PercentageComplete { get; set; } = 0;
    }
}
