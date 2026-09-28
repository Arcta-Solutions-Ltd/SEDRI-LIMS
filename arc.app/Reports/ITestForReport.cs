using arc.common.Models;
using arc.common.Models.Reports;
using arc.domain.Reports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports
{
    /// <summary>
    /// Interface for testing report functionalities.
    /// </summary>
    public interface ITestForReport
    {
        /// <summary>
        /// Gets a list of field names for the report.
        /// </summary>
        /// <returns>A list of field names as strings.</returns>
        List<string> GetListFields();

        /// <summary>
        /// Loads test data into the report asynchronously.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="testId">The ID of the test.</param>
        /// <param name="token">The token information model.</param>
        /// <param name="returnValue">The current report data to be returned.</param>
        /// <param name="reportCriteria">The criteria for the specimen selector list model.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the updated report data.</returns>
        Task<ReportData> LoadTestIntoReportAsync(string testName, int testId, TokenInfoModel token, ReportData returnValue, SpecimenSelectorListModel reportCriteria);
    }
}
