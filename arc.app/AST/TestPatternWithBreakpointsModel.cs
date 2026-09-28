using System.Collections.Generic;

namespace arc.app.AST
{
    public class TestPatternWithBreakpointsModel
    {
        public int Id { get; set; }
        public string TestPatternName { get; set; }
        public List<AntibioticLineWithBreakpointsModel> AntibioticGrid { get; set; }
    }
}
