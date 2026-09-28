using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Coding
{
    public interface ITestPatternRepository
    {
        Task<int> AddTestPatternAsync(TestPatternModel dataToSave);
        Task<int> EditTestPatternAsync(TestPatternModel dataToSave);
        Task DeleteTestPatternAsync(string id);
        Task<IEnumerable<TestPatternListModel>> GetTestPatternListAsync(QueryFilterConfig parameters);
        Task<IEnumerable<OptionsConfig>> GetTestPatternNamesListAsync();
        Task<IEnumerable<TestPatternScopeModel>> GetTestPatternsForOrganismListAsync(QueryFilterConfig parameters);
        Task<TestPattern> EditTestPatternQueryAsync(QueryFilterConfig queryFilters);
        Task<List<TestPatternLineModel>> GetTestPatternLinesAsync(QueryFilterConfig parameters);
    }
}
