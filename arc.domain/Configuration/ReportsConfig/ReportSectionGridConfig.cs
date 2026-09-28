using System.Collections.Generic;

namespace arc.domain.Configuration.ReportsConfig
{
    public class ReportSectionGridConfig
    {
        public string Name { get; set; }
        public List<string> Head { get; set; }

        /// <summary>
        /// When true, the PDF renderer omits cell borders for this grid table.
        /// </summary>
        public bool NoBox { get; set; }
    }
}
