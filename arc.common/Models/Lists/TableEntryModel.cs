namespace arc.common.Models.Lists
{
    public class TableEntryModel
    {
        public int ListId { get; set; }
        public string Value { get; set; }
        public string Enabled { get; set; }
        public int DisplayOrder { get; set; }
        public bool Fixed { get; set; }
        public string ParentId { get; set; }
    }
}
