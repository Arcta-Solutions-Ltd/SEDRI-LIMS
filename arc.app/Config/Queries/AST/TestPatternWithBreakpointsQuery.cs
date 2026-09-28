using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class TestPatternWithBreakpointsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'testpatternwithbreakpoints', 'Type': 'Special', Tablename: 'AST'}";
        }
    }
}
