using arc.app.Common;

namespace arc.app.Config.Queries.AST
{
    internal class BreakpointsForASTRowQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'breakpointsforastrow', 'Type': 'Special', Tablename: 'AST'}";
        }
    }
}
