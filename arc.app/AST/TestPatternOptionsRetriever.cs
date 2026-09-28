using arc.app.Coding;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public class TestPatternOptionsRetriever
    {
        private readonly ITestPatternRepository _testPatternRepository;

        public TestPatternOptionsRetriever(ITestPatternRepository testPatternRepository)
        {
            _testPatternRepository = testPatternRepository;
        }

        public async Task<List<TestPatternScopeModel>> GetTestPatternOptionsForCulture(ASTCultureModel cultureDetails)
        {
            var testPatternQueryFilters = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
                   {
                        new QueryValuesConfig() { Key = "OrganismId", Value = cultureDetails.OrganismId.ToString() },
                        new QueryValuesConfig() { Key = "OrgGroupCodingId", Value = cultureDetails.OrgGroupCodingId.ToString() },
                        new QueryValuesConfig() { Key = "specimentypeid", Value = cultureDetails.SpecimenTypeId.ToString() },
                   }
            };

            var testPatternResult = await _testPatternRepository.GetTestPatternsForOrganismListAsync(testPatternQueryFilters);
            var testPatternList = testPatternResult.ToList();
            return testPatternList;
        }

    }
}
