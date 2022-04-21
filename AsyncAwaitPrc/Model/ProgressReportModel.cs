using System.Collections.Generic;

namespace AsyncAwaitPrc.Model
{
    public class ProgressReportModel
    {
        public List<string> ProgressStatus { get; set; } = new();
        public int PercentageComplete { get; set; } = 0;
    }
}
