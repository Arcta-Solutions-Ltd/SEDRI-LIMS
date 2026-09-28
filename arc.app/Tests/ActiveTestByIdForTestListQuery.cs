using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace arc.app.Tests
{
    public class ActiveTestByIdForTestListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public ActiveTestByIdForTestListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            //var standardFilters = _serviceProvider.GetService<IStandardFilters>();

            //queryFilter = await standardFilters.ApplyTokenToQueryFiltersAsync(queryFilter, token);
            var activeTestList = await testRepository.GetActiveTestByIdForTestListAsync(queryFilter);

            return JsonConvert.SerializeObject(activeTestList);
        }
    }
}
