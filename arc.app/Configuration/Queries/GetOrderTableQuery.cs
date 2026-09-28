using arc.app.Common;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries
{
    internal class GetOrderTableQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetOrderTableQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metaflistid").First().Value;

            var listRepository = _serviceProvider.GetService<IListRepository>();

            var list = await listRepository.GetListValuesByIdAsync(int.Parse(id));

            var selectorValues = list.Select(p => new { id = p.Text, label = p.Text, value = p.Key });

            var result = new { Id = "ordertable", FieldList = selectorValues };

            return JsonConvert.SerializeObject(result);
        }
    }
}
