using System;

namespace arc.common.Models.Specimen
{
    public class CommentListModel
    {
        public string Id { get; set; }
        public string Comment { get; set; }
        public string DisplayOnReport { get; set; }
        public string CommentType { get; set; }
        public string AddedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
