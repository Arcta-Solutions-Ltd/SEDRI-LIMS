using arc.common.Models;
using System.Collections.Generic;

namespace arc.domain.Reports
{
    public class ReportData
    {
        public List<KeyValueModel> Standard { get; set; }
        public List<TableRow> Tables { get; set; }
        public List<GroupRow> Groups { get; set; }
    }
}
