using System.Collections.Generic;

namespace arc.common.Models.Reports
{
    public class SectionDefinitionModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ColumnDefinitionModel> Columns { get; set; }
        public List<GridDefinitionModel> Grids { get; set; }
    }
}
