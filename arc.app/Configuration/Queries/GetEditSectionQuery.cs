using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Reports;
using arc.common.Models.Config;
using Newtonsoft.Json;
using arc.domain.Configuration.ReportsConfig;
using arc.common;

namespace arc.app.Configuration.Queries
{
    internal class GetEditSectionQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditSectionQuery(IServiceProvider serviceProvider)
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

            // Translate the section to the display model
            var mapper = _serviceProvider.GetService<IMapType<ReportSectionConfig, SectionModel>>();
            var newSection = mapper.Map(section);

            //Output the display model
            return JsonConvert.SerializeObject(newSection);
        }
    }
}

