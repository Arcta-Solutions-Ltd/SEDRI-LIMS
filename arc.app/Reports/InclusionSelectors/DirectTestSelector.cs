using arc.app.Config.Forms;
using arc.app.Tests;
using arc.common.Models.Reports;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports.InclusionSelectors
{
    /// <summary>
    /// Selector for direct tests.
    /// </summary>
    public class DirectTestSelector : IDirectTestSelector
    {
        private readonly ITestRepository _testRepository;
        private readonly IFormConfigAdapter _formConfigAdapter;

        /// <summary>
        /// Initializes a new instance of the DirectTestSelector class.
        /// </summary>
        /// <param name="testRepository">The test repository instance.</param>
        /// <param name="formConfigAdapter">The form configuration adapter instance.</param>
        public DirectTestSelector(ITestRepository testRepository, IFormConfigAdapter formConfigAdapter)
        {
            _testRepository = testRepository;
            _formConfigAdapter = formConfigAdapter;
        }

        /// <summary>
        /// Asynchronously gets the contents for the direct test selector.
        /// </summary>
        /// <param name="specimenId">The ID of the specimen.</param>
        /// <returns>A list of DirectTestSelectorListModel objects.</returns>
        public async Task<List<DirectTestSelectorListModel>> GetContentsAsync(int specimenId)
        {
            var testList = await _testRepository.GetTestsForSpecimenAsync(specimenId);

            var returnList = new List<DirectTestSelectorListModel>();
            foreach (var test in testList)
            {
                var newEntry = new DirectTestSelectorListModel { Id = test.Id.ToString(), Name = _formConfigAdapter.GetFormAsync(test.TestName).Result.Title };

                if (test.TestResults != null)
                {
                    var testContents = JsonConvert.DeserializeObject<PrintOnReportModel>(test.TestResults);
                    newEntry.PrintOnReport = testContents.PrintOnReport;
                    returnList.Add(newEntry);
                }
            }
            return returnList;
        }
    }
}
