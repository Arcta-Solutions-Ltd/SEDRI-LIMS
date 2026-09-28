using arc.app.Common;
using arc.app.Laboratory;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Lite query handler for isolate test list on a culture: SQL + TAT + title-only mapping (no TestResults translation).
/// </summary>
public class TestListForCultureLiteQueryHandler : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates the handler with access to repository and mapping services.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public TestListForCultureLiteQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var totalStopwatch = Stopwatch.StartNew();
        var cultureId = queryFilter.GetIntegerValue("CultureId");

        logWriter?.LogInfo(
            $"testlistforculturelite start cultureId={cultureId}",
            nameof(TestListForCultureLiteQueryHandler),
            nameof(RunAsync));

        var testRepository = _serviceProvider.GetService<ITestRepository>();
        var specialMappingFactory = _serviceProvider.GetService<ISpecialMappingFactory>();

        var sqlStopwatch = Stopwatch.StartNew();
        var list = await testRepository.GetTestListForCultureAsync(cultureId);
        sqlStopwatch.Stop();

        var tatStopwatch = Stopwatch.StartNew();
        await TurnAroundTimeEnricher.EnrichTestListAsync(_serviceProvider, list, isCultureTest: true);
        tatStopwatch.Stop();

        var mapperStopwatch = Stopwatch.StartNew();
        var json = JsonConvert.SerializeObject(list);
        var mapper = specialMappingFactory.GetMapper("testlistforcultureliteresultmapper");
        var result = mapper.Map(json);
        mapperStopwatch.Stop();

        totalStopwatch.Stop();
        logWriter?.LogInfo(
            $"testlistforculturelite complete cultureId={cultureId} rowCount={list?.Count ?? 0} sqlMs={sqlStopwatch.ElapsedMilliseconds} tatMs={tatStopwatch.ElapsedMilliseconds} mapperMs={mapperStopwatch.ElapsedMilliseconds} totalMs={totalStopwatch.ElapsedMilliseconds}",
            nameof(TestListForCultureLiteQueryHandler),
            nameof(RunAsync));

        return result;
    }
}
