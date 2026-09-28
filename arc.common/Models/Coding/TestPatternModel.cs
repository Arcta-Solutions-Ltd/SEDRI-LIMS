using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class TestPatternModel : TestPatternWithoutCraftedModel
    {
        public List<CraftedModel> Crafted { get; set; }
    }
}
