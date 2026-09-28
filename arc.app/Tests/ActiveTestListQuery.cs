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
    /// <summary>
    /// Represents the ActiveTestListQuery class which runs a query to get the active test list.
    /// </summary>
    public class ActiveTestListQuery : IQueryRun
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveTestListQuery"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider to retrieve required services.</param>
        public ActiveTestListQuery(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Runs the query to get the active test list and returns the result as a JSON string.
        /// </summary>
        /// <param name="queryFilter">The query filter configuration.</param>
        /// <param name="token">The token information model.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the JSON string of the active test list.</returns>
        public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
        {
            var testRepository = _serviceProvider.GetService<ITestRepository>();
            var workflowAdapter = _serviceProvider.GetService<IWorkflowAdapter>();

            var workflow = await workflowAdapter.GetWorkflowAsync("specimendefault");

            var step = workflow.Steps.First(s => s.Event == "directtestentry");

            queryFilter.AddString("AllowedStates", step.EntryState);
            queryFilter.AddString("Type", "direct");

            var activeTestList = await testRepository.GetActiveTestListAsync(queryFilter, token);

            await TurnAroundTimeEnricher.EnrichTestListAsync(_serviceProvider, activeTestList, isCultureTest: false);

            return JsonConvert.SerializeObject(activeTestList);
        }
    }
}
