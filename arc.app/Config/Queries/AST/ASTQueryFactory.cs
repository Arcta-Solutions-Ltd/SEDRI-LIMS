using arc.app.Common;
using arc.app.Config.Queries.AST;

namespace arc.app.Config.Queries
{
    internal class ASTQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "astaddquery" => new ASTAddQuery(),
                "getculturetestsforculture" => new GetCultureTestsForCultureQuery(),
                "astheaderforastreport" => new ASTHeaderForASTReportQuery(),
                "astlistbycultureid" => new ASTListByCultureIdQuery(),
                "astlistforastreport" => new ASTListForASTReportQuery(),
                "astlistwide" => new ASTListWideQuery(),
                "breakpointsforastrow" => new BreakpointsForASTRowQuery(),
                "testpatternwithbreakpoints" => new TestPatternWithBreakpointsQuery(),
                _ => null,
            };
        }
    }
}
