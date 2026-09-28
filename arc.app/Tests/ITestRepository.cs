using arc.common;
using arc.common.Models;
using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests;

public interface ITestRepository
{
    Task<Test> GetTestAsync(int id);
    Task<Test> GetCultureTestAsync(int id);
    Task<IEnumerable<Test>> GetTestsForSpecimenAsync(int specimenId);
    Task<IEnumerable<Test>> GetTestsForCultureAsync(string cultureId);
    Task AddTestsAsync(string dataToSave, string id, EventModel command);
    Task<int> AddCultureTestsAsync(string dataToSave, string id);
    Task<List<CultureTest>> GetTestsForCultureAsync(QueryFilterConfig queryFilters);
    Task<int> DirectTestUsageCountAsync(QueryFilterConfig parameters);
    Task<int> CultureTestUsageCountAsync(QueryFilterConfig parameters);
    Task<List<TestListResultModel>> GetActiveTestListAsync(QueryFilterConfig parameters, TokenInfoModel token);
    Task<List<TestListResultModel>> GetTestListForSpecimenAsync(int specimenId);
    Task<List<TestListResultModel>> GetTestListForCultureAsync(int cultureId);
    Task<TestListResultModel> GetActiveTestByIdForTestListAsync(QueryFilterConfig queryFilters);
    Task<TestListResultModel> GetActiveCultureTestByIdForTestListAsync(QueryFilterConfig queryFilters);
    /// <summary>
    /// Gets the culturetest ID for the given culture and test name. Creates the record if it does not exist.
    /// </summary>
    Task<int> GetOrCreateCultureTestIdAsync(int cultureId, string testName);

    /// <summary>
    /// Returns an existing <c>Tests</c> row id for the specimen and test name, or inserts one (does not trigger instrument pending rows).
    /// </summary>
    Task<int> EnsureDirectTestExistsAsync(int specimenId, string testName);

    /// <summary>
    /// Returns an existing <c>CultureTests</c> row id or inserts one (does not trigger instrument pending rows).
    /// </summary>
    Task<int> EnsureCultureTestExistsAsync(int cultureId, string testName);
}
