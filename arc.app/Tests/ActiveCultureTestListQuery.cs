using arc.app.Common;
using arc.app.Config.Workflows;
using arc.app.Laboratory;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    internal class ActiveCultureTestListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        public ActiveCultureTestListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();

            var workflow = await workflowAdapter.GetWorkflowAsync("specimendefault");
            var step = workflow.Steps.First(s => s.Event == "directtestentry");
            queryFilter.AddString("AllowedStates", step.EntryState);
            queryFilter.AddString("Type", "culture");

            var activeTestList = await testRepository.GetActiveTestListAsync(queryFilter, token);

            await TurnAroundTimeEnricher.EnrichTestListAsync(_serviceProvider, activeTestList, isCultureTest: true);

            return JsonConvert.SerializeObject(activeTestList);
        }
    }
}

