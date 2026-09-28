using System.Collections.Generic;

namespace arc.common.Models.Lists
{
    public class TableEntryCraftedModel
    {
        public string Id { get; set; }
        public string Event { get; set; }
        public int MetafListId { get; set; }
        public List<TableEntryCraftedContentsModel> Crafted { get; set; }
    }
}
