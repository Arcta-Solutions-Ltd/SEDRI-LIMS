using arc.app.Common;
using arc.app.Reports;
using arc.app.SystemConfig;
using arc.app.Utils;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Actions
{
    /// <summary>
    /// Executes the logic for publishing a specimen report and recording it in the report history.
    /// </summary>
    internal class PublishReportAction : IAction
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishReportAction"/> class.
        /// </summary>
        /// <param name="serviceProvider">The dependency injection container used to resolve services.</param>
        internal PublishReportAction(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Publishes the report for the given specimen and saves it to the report history.
        /// </summary>
        /// <param name="queryFilter">Query parameters containing report metadata.</param>
        /// <param name="token">Authentication and context information including the language and laboratory.</param>
        public async Task Do(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var specimenId = queryFilter.Parameters
                .First(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase));

            var reportName = queryFilter.Parameters
                .First(p => p.Key.Equals("reportname", StringComparison.OrdinalIgnoreCase));

            var reportConfig = queryFilter.Parameters
                .First(p => p.Key.Equals("reportconfig", StringComparison.OrdinalIgnoreCase));

            var reportApproval = queryFilter.Parameters
                .FirstOrDefault(p => p.Key.Equals("doreportsneedapproval", StringComparison.OrdinalIgnoreCase));

            var languageHandler = _serviceProvider.GetService<ILanguageHandler>();
            var translatedName = await languageHandler.TranslateAsync(reportName.Value, token.LanguageId);

            var reportHandler = _serviceProvider.GetService<ISpecimenRecordReportHandler>();
            var reportContents = await reportHandler.HandleAsync(queryFilter, token);
            reportContents = await languageHandler.TranslateAsync(reportContents, token.LanguageId);

            var configQueryFilter = new QueryFilterConfig
            {
                Parameters = new List<QueryValuesConfig>
            {
                new QueryValuesConfig { Key = "configname", Value = reportConfig.Value }
            }
            };

            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            var reportId = await configRepository.SingleConfigByNameAsync(configQueryFilter);

            var doReportsNeedApproval = reportApproval != null && reportApproval.Value.Equals("yes", StringComparison.OrdinalIgnoreCase);

            var reportHistory = new ReportHistory
            {
                Id = 1,
                SpecimenId = int.Parse(specimenId.Value),
                Name = translatedName,
                ReportId = reportId.Id,
                Contents = reportContents,
                ReportConfig = reportConfig.Value,
                ReportApprovalId = doReportsNeedApproval ? 122 : 123,
                Username = token.Username,
                IncludeApprovalInfo = !doReportsNeedApproval
            };

            var reportHistoryRepository = _serviceProvider.GetService<IReportHistoryRepository>();
            await reportHistoryRepository.AddReportHistoryAsync(reportHistory);
        }
    }
}
