using System;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Common;
using arc.app.Config.Forms;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace arc.app.Tests
{
    public class RemoveDirectTestQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveDirectTestQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
        {
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            var formConfigAdapter = _serviceProvider.GetService<IFormConfigAdapter>();

            var id = queryFilter.Parameters.Where(p => p.Key.ToLower() == "id").First();
            var test = await testRepository.GetTestAsync(int.Parse(id.Value));
            var testform = await formConfigAdapter.GetFormAsync(test.TestName);

            var returnValue = new { Id = id, testform.Title, test.Status };
            return JsonConvert.SerializeObject(returnValue);
        }
    }
}
