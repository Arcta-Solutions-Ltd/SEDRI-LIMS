using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Newtonsoft.Json;
using arc.app.Config.Reports.DataSection;

namespace arc.app.Configuration.Queries
{
    internal class GetFieldSelectionListQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetFieldSelectionListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var sectionAdapter = _serviceProvider.GetService<IDataSectionAdapter>();

            var source = queryFilters.Parameters.Where(p => p.Key.ToLower() == "source").First().Value;

            var section = await sectionAdapter.GetSectionAsync(source);

            var returnList = new List<object>();
            foreach (var field in section.Fields)
            {
                var line = new { label = field.Label, value = field.Value };
                returnList.Add(line);
            }

            return JsonConvert.SerializeObject(returnList);
        }
    }
}
