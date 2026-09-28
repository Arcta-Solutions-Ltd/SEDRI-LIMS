using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Coding;

/// <summary>
/// Executes the breakpoint view query. Loads a single breakpoint by the criteria in the query filter
/// and returns it as JSON for display on the breakpoint view/record screen.
/// </summary>
/// <remarks>
/// Resolves <see cref="IBreakpointRepository"/> from the service provider and calls
/// BreakpointViewByIdQueryAsync; the result is serialized to JSON for the client.
/// </remarks>
internal class BreakpointViewQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes the query runner with the application service provider.
    /// </summary>
    /// <param name="serviceProvider">Used to resolve <see cref="IBreakpointRepository"/> at runtime.</param>
    public BreakpointViewQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the breakpoint view query using the given filter and returns the breakpoint as JSON.
    /// </summary>
    /// <param name="queryFilter">Filter configuration (e.g. breakpoint id) for the query.</param>
    /// <param name="token">Token/culture context; not used by this query but required by the interface.</param>
    /// <returns>JSON-serialized breakpoint entity, or empty/null representation if not found.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var breakpointRepository = _serviceProvider.GetService<IBreakpointRepository>();

        var breakpoint = await breakpointRepository.BreakpointViewByIdQueryAsync(queryFilter);

        return JsonConvert.SerializeObject(breakpoint);
    }
}
