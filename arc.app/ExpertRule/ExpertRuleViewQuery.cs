using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Executes the expert rule view query. Loads a single expert rule by the criteria in the query filter
/// and returns it as JSON for display on the expert rule view/record screen.
/// </summary>
/// <remarks>
/// Resolves <see cref="IExpertRuleRepository"/> from the service provider and calls
/// ExpertRuleViewByIdQueryAsync; the result is serialized to JSON for the client.
/// </remarks>
internal class ExpertRuleViewQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes the query runner with the application service provider.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve <see cref="IExpertRuleRepository"/> at runtime.</param>
    public ExpertRuleViewQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the expert rule view query using the given filter and returns the expert rule as JSON.
    /// </summary>
    /// <param name="queryFilter">Filter configuration (e.g. expert rule id) for the query.</param>
    /// <param name="token">Token/culture context; not used by this query but required by the interface.</param>
    /// <returns>JSON-serialized expert rule entity, or empty/null representation if not found.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var expertRuleRepository = _serviceProvider.GetService<IExpertRuleRepository>();

        var expertRule = await expertRuleRepository.ExpertRuleViewByIdQueryAsync(queryFilter);

        return JsonConvert.SerializeObject(expertRule);
    }
}
