using arc.app.Common;
using arc.app.Config.Reports;
using arc.app.Reports.InclusionSelectors;
using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Reports
{
    /// <summary>
    /// Handles the request for a report.
    /// </summary>
    public class ReportHandler : IReportHandler
    {
        private readonly IReportFactory _reportFactory;
        private readonly ISpecimenRecordReportHandler _specimenRecordReportHandler;
        private readonly IReportHistoryRepository _reportHistoryRepository;
        private readonly IReportInclusionSelector _reportInclusionSelector;
        private readonly ICultureDetailsSelector _cultureDetailsSelector;
        private readonly IReportRepository _reportRepository;
        private readonly IStandardFilters _standardFilters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportHandler"/> class.
        /// </summary>
        /// <param name="reportFactory">The report factory.</param>
        /// <param name="specimenRecordReportHandler">The specimen record report handler.</param>
        /// <param name="reportHistoryRepository">The report history repository.</param>
        /// <param name="reportInclusionSelector">The report inclusion selector.</param>
        /// <param name="cultureDetailsSelector">The culture details selector.</param>
        /// <param name="reportRepository">The report repository.</param>
        /// <param name="standardFilters">The standard filters.</param>
        public ReportHandler(
            IReportFactory reportFactory,
            ISpecimenRecordReportHandler specimenRecordReportHandler,
            IReportHistoryRepository reportHistoryRepository,
            IReportInclusionSelector reportInclusionSelector,
            ICultureDetailsSelector cultureDetailsSelector,
            IReportRepository reportRepository,
            IStandardFilters standardFilters)
        {
            _reportFactory = reportFactory;
            _specimenRecordReportHandler = specimenRecordReportHandler;
            _reportHistoryRepository = reportHistoryRepository;
            _reportInclusionSelector = reportInclusionSelector;
            _cultureDetailsSelector = cultureDetailsSelector;
            _reportRepository = reportRepository;
            _standardFilters = standardFilters;
        }

        /// <summary>
        /// Asynchronously handles the specified contents and query filters.
        /// </summary>
        /// <param name="contents">The contents.</param>
        /// <param name="queryFilters">The query filter configuration parameters.</param>
        /// <param name="queryData">The query configuration data.</param>
        /// <param name="token">The token information model.</param>
        /// <param name="excludeTokenFilters">If set to <c>true</c>, exclude token filters.</param>
        /// <returns>A task that represents the asynchronous operation and contains the result as a string.</returns>
        public async Task<string> HandleAsync(
            string contents,
            QueryFilterConfig queryFilters,
            QueryConfig queryData,
            TokenInfoModel token,
            bool excludeTokenFilters = false)
        {
            var reportHandler = _reportFactory.GetReport(queryFilters.Name);
            var config = reportHandler.Get();

            string result = "";

            switch (queryFilters.Name.ToLower())
            {
                case "antibiogramquery":
                    queryFilters.AddTokenFilter("specimen", token);
                    //queryFilters = await _standardFilters.ApplyTokenToQueryFiltersAsync(queryFilters, token);
                    var antibiogramResults = await _reportRepository.GetAntibiogramDataAsync(queryFilters);
                    result = JsonConvert.SerializeObject(antibiogramResults);
                    break;
                case "editcultureprintselectionquery":
                    var cultureDetails = await _cultureDetailsSelector.GetContentsAsync(queryFilters);
                    result = JsonConvert.SerializeObject(cultureDetails);
                    break;
                case "admissionreportlist":
                    var admissionReportList = await _reportHistoryRepository.GetAdmissionReportListAsync(queryFilters);
                    result = JsonConvert.SerializeObject(admissionReportList);
                    break;
                case "patientreportlist":
                    var patientReportList = await _reportHistoryRepository.GetPatientReportListAsync(queryFilters);
                    result = JsonConvert.SerializeObject(patientReportList);
                    break;
                case "requestreportlist":
                    var requestReportList = await _reportHistoryRepository.GetRequestReportListAsync(queryFilters);
                    result = JsonConvert.SerializeObject(requestReportList);
                    break;
                case "reportcontents":
                    var reportContents = await _reportHistoryRepository.GetReportContentsAsync(queryFilters);
                    result = JsonConvert.SerializeObject(reportContents);
                    break;
                case "reportfiltercontents":
                    result = await _reportInclusionSelector.GetFilterListsAsync(queryFilters);
                    break;
                case "specimenrecordreport":
                    var specimenRecord = await _specimenRecordReportHandler.HandleAsync(queryFilters, token);
                    result = JsonConvert.SerializeObject(specimenRecord);
                    break;
            }

            return result;
        }
    }

}
