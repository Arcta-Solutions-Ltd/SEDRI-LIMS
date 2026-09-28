using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.SystemConfig;

/// <summary>
/// Represents a query that retrieves a list of workflows and returns the result as a JSON string.
/// </summary>
internal class WorkflowListQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowListQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    internal WorkflowListQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the query asynchronously, retrieves the workflow list from the configuration repository,
    /// and returns the result as a serialized JSON string.
    /// </summary>
    /// <param name="queryFilter">The query filter configuration (unused in this implementation).</param>
    /// <param name="token">The token information for authentication/authorization (unused in this implementation).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a JSON string representing the list of workflows.
    /// </returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var configRepository = _serviceProvider.GetService<IConfigRepository>();
        var result = await configRepository.GetWorkflowListAsync();
        return JsonConvert.SerializeObject(result);
    }
}

