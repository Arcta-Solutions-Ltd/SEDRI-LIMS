using System.Collections.Generic;

namespace arc.common.Models.Export
{
    public class WNHeader
    {
        public int CultureId { get; set; }
        public string SpecimenId { get; set; } //Need to delete
        public string Organism { get; set; }
        public List<WNExportList> Antibiotics { get; set; } = new List<WNExportList>();
    }

    public class WNExportList
    {
        public string ColumnCode { get; set; }
        public string Measurement { get; set; }
        public string QualitativeValue { get; set; }
    }
}
