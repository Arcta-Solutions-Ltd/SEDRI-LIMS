using arc.app.Common;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Config.Forms;
using System.Linq;
using Newtonsoft.Json;
using arc.common.Models;

namespace arc.app.Tests
{
    internal class RemoveCultureTestQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveCultureTestQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
        {
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            var formConfigAdapter = _serviceProvider.GetService<IFormConfigAdapter>();

            var id = queryFilter.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var test = await testRepository.GetCultureTestAsync(int.Parse(id.Value));
            var testform = await formConfigAdapter.GetFormAsync(test.TestName);

            var returnValue = new { Id = id, testform.Title, test.Status };
            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
