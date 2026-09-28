using arc.common.Models.AST;
using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class BreakpoinReturnModel
    {
        public List<SusceptibilityModel> Susceptibilities { get; set; }
        public List<TestPatternLineModel> TestPatternLines { get; set; }
        public List<ExpertRuleActionReturnModel> ExpertRuleActions { get; set; }

    }
}
