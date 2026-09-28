using arc.app.Config.Reports;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace arc.app.Configuration.Queries
{
    internal class GetDeleteSectionQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetDeleteSectionQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            // Get the id of section to edit.
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");
            // Get the section from the database
            var sectionAdapter = _serviceProvider.GetService<ISectionAdapter>();
            var section = await sectionAdapter.GetSectionAsync(idList[1]);

            var returnValue = new { Id = id, Name = section.Description };

            //Output the display model
            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
