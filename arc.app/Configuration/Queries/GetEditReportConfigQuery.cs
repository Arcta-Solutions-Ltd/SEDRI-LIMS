using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using arc.app.Config.Reports;
using Newtonsoft.Json;

namespace arc.app.Configuration.Queries
{
    internal class GetEditReportConfigQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditReportConfigQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var report = await reportAdapter.GetReportAsync(idList[1]);

            var configExtractionUtils = _serviceProvider.GetService<IConfigExtractionUtils>();
            var view = await configExtractionUtils.GetViewConfigUsingIdAsync(int.Parse(idList[0]));
            var enabled = view.Reports.Contains(idList[1]) ? "Yes" : "No";


            var returnValue = new { Id = report.Name, Title = report.Title, Enabled = enabled };
            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
