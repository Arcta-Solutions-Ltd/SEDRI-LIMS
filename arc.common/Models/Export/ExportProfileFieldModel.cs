using System;

namespace arc.common.Models.Export
{
    public class ExportProfileFieldModel
    {
        public int Id { get; set; }
        public int ExportProfileId { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FormName { get; set; }
        public string LabelName { get; set; }
        public string HeaderName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int OrderNumber { get; set; }
        public string MoreData { get; set; }
        public string Mapping { get; set; }
    }
}
