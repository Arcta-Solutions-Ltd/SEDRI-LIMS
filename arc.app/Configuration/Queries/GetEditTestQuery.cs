using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Forms;
using Newtonsoft.Json;

namespace arc.app.Configuration.Queries
{
    internal class GetEditTestQuery : ISingleConfig
    {
        private readonly IServiceProvider _serviceProvider;

        internal GetEditTestQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First().Value;
            var idList = id.Split("|");

            var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();
            var form = await formAdapter.GetFormAsync(idList[1]);

            var returnValue = new { Id = id, Title = form.Title };
            return JsonConvert.SerializeObject(returnValue);
        }

    }
}
