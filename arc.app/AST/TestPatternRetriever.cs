using arc.app.Coding;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.AST
{
    public class TestPatternRetriever
    {
        private readonly ITestPatternRepository _testPatternRepository;
        //private readonly IBreakpointRepository _breakpointRepository;

        public TestPatternRetriever(ITestPatternRepository testPatternRepository/*, IBreakpointRepository breakpointRepository*/)
        {
            _testPatternRepository = testPatternRepository;
            //_breakpointRepository = breakpointRepository;
        }

        public async Task<TestPatternWithBreakpointsModel> GetTestPatternWithBreakpoints(string testPatternId, ASTCultureModel cultureDetails)
        {
            // Get test-pattern definition.
            var testPatternQueryFilters = new QueryFilterConfig();

            testPatternQueryFilters.Parameters = new List<QueryValuesConfig> {
                new QueryValuesConfig {
                    Key = "Id",
                    Value = testPatternId
                }
            };
            var testPatternDefn = await _testPatternRepository.EditTestPatternQueryAsync(testPatternQueryFilters);

            var testPatternWithBreakpoints = new TestPatternWithBreakpointsModel
            {
                Id = testPatternDefn.Id,
                TestPatternName = testPatternDefn.TestPatternName,
                AntibioticGrid = new List<AntibioticLineWithBreakpointsModel>()
            };

            foreach (var testPatternLine in testPatternDefn.AntibioticGrid)
            {
                var antibioticLine = new AntibioticLineWithBreakpointsModel
                {
                    Id = testPatternLine.Id,
                    testOrder = testPatternLine.testOrder,
                    AntibioticId = testPatternLine.AntibioticId,
                    Dosage = testPatternLine.Dosage,
                    TestMethodId = testPatternLine.TestMethodId,
                    GuidelinesId = testPatternLine.GuidelinesId,
                    CategoryId = testPatternLine.CategoryId,
                    PrintOnReport = testPatternLine.PrintOnReport ? "Yes" : "No",
                    //breakpoints = new List<Breakpoint>()
                };

                // Get breakpoints for relevant criteria.

                //   var breakpointCriteria = new BreakpointCriteriaModel
                //   {
                //       SpecimenTypeId = cultureDetails.SpecimenTypeId,
                //       OrganismId = cultureDetails.OrganismId,
                //       OrgGroupCodingId = cultureDetails.OrgGroupCodingId,
                //       AntibioticId = (int)antibioticLine.AntibioticId,
                //       TestMethodId = (int)antibioticLine.TestMethodId,
                //       SourceId = (int)antibioticLine.GuidelinesId,
                //       Dosage = antibioticLine.Dosage
                //   };

                //    var breakpointRetriever = new BreakpointRetriever(_breakpointRepository);
                //    var breakpoints = await breakpointRetriever.GetBreakpointsFromCriteria(breakpointCriteria);

                //    foreach (var breakpoint in breakpoints)
                //    {
                //        if (breakpoint != null)
                //        {
                //            antibioticLine.breakpoints.Add(breakpoint);
                //        }
                //    }
                testPatternWithBreakpoints.AntibioticGrid.Add(antibioticLine);
            }
            return testPatternWithBreakpoints;
        }
    }
}
