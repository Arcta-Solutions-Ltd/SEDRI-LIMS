using arc.common.Models;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Threading.Tasks;

namespace arc.app.Reports
{
    public interface ISpecimenRecordReportHandler
    {
        /// <summary>
        /// Gets or sets the report criteria for the specimen record.
        /// </summary>
        SpecimenSelectorListModel ReportCriteria { get; set; }

        /// <summary>
        /// Asynchronously handles the specified query filter parameters and token information,
        /// and returns the result as a string.
        /// </summary>
        /// <param name="parameters">The query filter configuration parameters.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result as a string.</returns>
        Task<string> HandleAsync(QueryFilterConfig parameters, TokenInfoModel token);
    }
}
