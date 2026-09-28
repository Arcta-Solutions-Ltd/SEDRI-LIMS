namespace arc.common.Models.Specimen
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public string Value { get; set; }
        public string SpecimenId  { get; set; }
        public string CultureId { get; set;}
        public string FieldId { get; set; }
        public string DisplayOnReport { get; set; }
        public int CannedCommentId { get; set; }

        public bool IsCultureComment()
        {
            return ! string.IsNullOrEmpty(CultureId);
        }
    }
}
