using arc.app.Config.Reports;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace arc.app.Configuration.Queries
{
    internal class GetDeleteReportConfigQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetDeleteReportConfigQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var reportAdapter = _serviceProvider.GetService<IReportAdapter>();
            var report = await reportAdapter.GetReportAsync(idList[1]);

            var returnValue = new { Id = report.Name, Title = report.Title };
            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
