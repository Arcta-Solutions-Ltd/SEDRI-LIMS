using System.Collections.Generic;

namespace arc.common.Models.Reports
{
    public class GridLineContentsModel
    {
        public string Label { get; set; }
        public List<string> Contents { get; set; } = new List<string>();
    }
}
