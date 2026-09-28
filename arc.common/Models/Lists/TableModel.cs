namespace arc.common.Models.Lists
{
    public class TableModel
    {
        public string ListId { get; set; }
        public string Name { get; set; }       
        public string Grouping { get; set; }
        public string ParentId { get; set; }
        public bool Common { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
    }
}
