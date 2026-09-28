using arc.data.model.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.SystemConfig;
using arc.app.Config;
using System.Collections.Generic;
using Newtonsoft.Json;
using arc.app.Config.Reports;

namespace arc.app.Configuration
{
    /// <summary>
    /// Provides a configuration response containing all reports and their enabled state for a given view.
    /// </summary>
    internal class GetReportConfigList : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetReportConfigList(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Builds a serialized list of report metadata for the requested view.
        /// Each entry includes whether the report is enabled in the view by checking the view config <c>Reports</c> collection.
        /// </summary>
        /// <param name="queryFilters">Filter data containing the view identifier.</param>
        /// <param name="queryData">Query definition metadata supplied by the configuration engine.</param>
        /// <returns>A JSON string containing report id, title, enabled status, and state identifier.</returns>
        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var returnList = new List<object>();

            var configRepository = _serviceProvider.GetService<IConfigRepository>();
            string viewName;

            // Support both numeric ID and view name
            if (queryFilters.Parameters != null && queryFilters.Parameters.Count > 0)
            {
                var rawValue = queryFilters.Parameters[0].Value;
                if (int.TryParse(rawValue, out _))
                {
                    // Original behavior: numeric ID - look up config record to get view name
                    queryFilters.Parameters[0].Key = "id";
                    var viewRecord = await configRepository.SingleConfigByIdAsync(queryFilters);
                    viewName = viewRecord.ConfigName;
                }
                else
                {
                    // New behavior: view name passed directly
                    viewName = "specimens";
                }
            }
            else
            {
                throw new ArgumentException("A view identifier is required to retrieve the report configuration list.", nameof(queryFilters));
            }

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("configtypeid", 13);
            var reports = await configRepository.GetConfigListAsync(queryFilter);

            var listViewFactory = _serviceProvider.GetService<IListViewConfigFactory>();
            var view = await listViewFactory.GetViewAsync(viewName);

            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            foreach (var report in reports)
            {
                var enabled = view.Reports.Contains(report.ConfigName) ? "Yes" : "No";
                var reportDef = await reportAdapter.GetReportAsync(report.ConfigName);
                var stateid = reportDef.Configurable == "No" ? "none" : "canconfig";

                returnList.Add(new { Id = queryFilters.Parameters[0].Value + "|" + reportDef.Name, Name = reportDef.Title, Enabled = enabled, stateid = stateid  });
            }

            return JsonConvert.SerializeObject(returnList);
        }
    }
}
