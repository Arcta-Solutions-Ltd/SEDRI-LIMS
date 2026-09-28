namespace arc.domain.Coding
{
    public class ListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Grouping { get; set; }    
        public int ParentId { get; set; }
        public bool Common { get; set; }
        public string Description { get; set; }
    }
}
