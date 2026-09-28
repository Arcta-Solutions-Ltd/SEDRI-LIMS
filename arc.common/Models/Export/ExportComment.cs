namespace arc.common.Models.Export
{
    public class ExportComment
    {
        public int? SpecimenId { get; set; }
        public int? CultureId { get; set; }
        public int CommentTypeId { get; set; }
        public string Comment { get; set; } = null;
        public string CannedComment { get; set; } = null;
    }
}
