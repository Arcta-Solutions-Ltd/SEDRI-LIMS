using arc.common.Models;
using arc.common.Models.Role;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    public interface ITestSelectionHandler
    {
        Task<string> GetTestsForSpecimenAsync(QueryFilterConfig queryFilters);
        Task<string> GetAllTestsAsync(string testType, TokenInfoModel token);
        Task<string> GetAllTestsWithPatientRefAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token);
        Task<string> GetTestsAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token);
        Task<string> GetTestsAndCultureTypesAsync(string testType, TokenInfoModel token);
        Task<string> GetTestsAndCultureTypesWithPatientRefAsync(QueryFilterConfig parameters, string testType, TokenInfoModel token);
        JustCraftedPages CreateReturnModel(List<CraftedSelectionsModel> testList, string testType);
        Task<List<CraftedSelectionsModel>> GetTestListWithUsedTestsRemovedAsync(string id, string testType, TokenInfoModel token);
        Task<List<CraftedSelectionsModel>> GetTestListWithUsedTestsEnabledAsync(string id, string testType, TokenInfoModel token);
        Task<List<CraftedSelectionsModel>> GetCultureTypeListAsync();
    }
}
