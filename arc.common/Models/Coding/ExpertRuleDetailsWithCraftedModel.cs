using System.Collections.Generic;

namespace arc.common.Models.Coding;
public class ExpertRuleDetailsWithCraftedModel : ExpertRuleDetailsModel
{
    public List<CraftedModel> Crafted { get; set; }
    public int ContainsOrganism { get; set; }
}
