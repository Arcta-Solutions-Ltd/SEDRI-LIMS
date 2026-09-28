using arc.common.Models.AST;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;
using System.Collections.Generic;
using arc.common.Models.Coding;

namespace arc.app.AST
{
    public interface IASTHandler
    {
        Task<string> GetASTDataForCultureAsync(QueryFilterConfig queryFilters);
        Task<ASTRowModel> GetSusceptibilitiesAndExpertRules(ASTRowModel row);

        /// <summary>
        /// Returns expert rule groups and flat result rows for the AST page from the supplied disk/MIC state (page-scoped evaluation).
        /// </summary>
        Task<ExpertRulePageDisplayResult> GetExpertRulesForPageAsync(ExpertRulesForPageRequest request);
        //Task<string> GetTestPatternWithBreakpointsAsync(QueryFilterConfig queryFilters);
        //Task<string> GetBreakpointsForASTRowAsync(QueryFilterConfig queryFilters);
        Task<List<SusceptibilityModel>> GetSusceptibilitiesAsync(QueryFilterConfig queryFilters);
        Task<List<TestPatternLineModel>> GetTestPatternLinesAsync(string parameters);
    }
}
