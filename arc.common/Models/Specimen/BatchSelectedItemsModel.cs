using System.Collections.Generic;

namespace arc.common.Models.Specimen
{
    public class BatchSelectedItemsModel
    {
        public List<BatchSelectedItemModel> SelectedItems { get; set; }
    }

    public class BatchSelectedItemModel
    {
        public int Id { get; set; }
        public int StateId { get; set; }
    }
}
