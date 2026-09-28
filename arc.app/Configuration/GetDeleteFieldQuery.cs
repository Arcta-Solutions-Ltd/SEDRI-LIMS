using arc.app.Config.Pages;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace arc.app.Configuration
{
    internal class GetDeleteFieldQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetDeleteFieldQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var pageAdapter = _serviceProvider.GetService<IPageConfigAdapter>();
            var page = await pageAdapter.GetPageAsync(idList[1]);

            foreach (var column in page.Columns)
            {
                foreach (var formgroup in column.FormGroups)
                {
                    foreach (var field in formgroup.Fields)
                    {
                        if (field.Id == idList[2])
                        {
                            var returnValue = new
                            {
                                Name = field.Id
                            };
                            return JsonConvert.SerializeObject(returnValue);
                        }
                    }
                }
            }


            return JsonConvert.SerializeObject("{}");
        }
    }
}
