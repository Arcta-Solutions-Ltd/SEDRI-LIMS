using arc.common.Models.Config;
using System.Collections.Generic;

namespace arc.common.Models.Lists
{
    public class OrderTableModel
    {
        public string MetafListId { get; set; }
        public List<FieldListModel> FieldList { get; set; }
    }
}
