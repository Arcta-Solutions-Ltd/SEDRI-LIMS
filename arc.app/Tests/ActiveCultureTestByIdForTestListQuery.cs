using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace arc.app.Tests;

/// <summary>
/// Handles execution of a query to retrieve active culture tests by ID for a test list,
/// applying token-based filters before querying the repository.
/// </summary>
public class ActiveCultureTestByIdForTestListQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActiveCultureTestByIdForTestListQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider for resolving dependencies such as repositories and filter utilities.</param>
    public ActiveCultureTestByIdForTestListQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query by applying token-based filters and retrieving matching culture test data.
    /// </summary>
    /// <param name="queryFilter">Query filter containing selection parameters.</param>
    /// <param name="token">Token information used for scoping or security filtering.</param>
    /// <returns>A serialized JSON string representing the list of active culture tests.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var testRepository = _serviceProvider.GetService<ITestRepository>();
        //var standardFilters = _serviceProvider.GetService<IStandardFilters>();

        //queryFilter = await standardFilters.ApplyTokenToQueryFiltersAsync(queryFilter, token);
        var activeTestList = await testRepository.GetActiveCultureTestByIdForTestListAsync(queryFilter);

        return JsonConvert.SerializeObject(activeTestList);
    }
}
