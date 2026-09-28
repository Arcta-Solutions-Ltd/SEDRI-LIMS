using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class BreakpointValidatorModel
    {
        public int SpecificationId { get; set; }
        public List<BreakpointLineModel> BreakpointGrid { get; set; }
    }
}
