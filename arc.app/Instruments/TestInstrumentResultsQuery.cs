using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Runs the test-scoped instrument results query for the test record view embedded list.
/// </summary>
internal class TestInstrumentResultsQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestInstrumentResultsQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
    public TestInstrumentResultsQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <summary>
    /// Runs the query asynchronously. Parameters: <c>id</c>, <c>source</c> (direct or culture).
    /// </summary>
    /// <param name="queryFilter">The query filter configuration.</param>
    /// <param name="token">The token information model.</param>
    /// <returns>JSON array of instrument result list rows.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        if (queryFilter == null)
            throw new ArgumentNullException(nameof(queryFilter));

        var instrumentRepository = _serviceProvider.GetRequiredService<IInstrumentRepository>();
        var instrumentList = await instrumentRepository.GetTestInstrumentResultsAsync(queryFilter);

        return JsonConvert.SerializeObject(instrumentList);
    }
}
