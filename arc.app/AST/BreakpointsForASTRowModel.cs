using arc.common.Models.Coding;
using arc.domain.Coding;
using System.Collections.Generic;

namespace arc.app.AST
{
    public class BreakpointsForASTRowModel
    {
        public string Handle { get; set; }
        //public int BreakpointId { get; set; }
        //public List<BreakpointLine> BreakpointLines { get; set; }
        public List<Breakpoint> Breakpoints { get; set; }
    }
}
