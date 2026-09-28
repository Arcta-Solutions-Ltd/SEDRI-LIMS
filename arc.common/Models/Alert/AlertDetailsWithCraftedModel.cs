using System.Collections.Generic;

namespace arc.common.Models.Alert
{
    public class AlertDetailsWithCraftedModel : AlertDetailsModel
    {
        public List<CraftedModel> Crafted { get; set; }
        public int ContainsOrganism { get; set; }
    }
}
