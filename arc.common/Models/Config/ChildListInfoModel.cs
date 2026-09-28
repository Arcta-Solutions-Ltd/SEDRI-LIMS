namespace arc.common.Models.Config
{
    /// <summary>
    /// Describes a list that has a parent list in the table hierarchy.
    /// </summary>
    public class ChildListInfoModel
    {
        public int ListId { get; set; }
        public int ParentListId { get; set; }
        public string ParentListName { get; set; }
    }
}
