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
/// Lite query handler for direct test list on a specimen: SQL + TAT + title-only mapping (no TestResults translation).
/// </summary>
public class TestListForSpecimenLiteQueryHandler : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Creates the handler with access to repository and mapping services.
    /// </summary>
    /// <param name="serviceProvider">Application service provider.</param>
    public TestListForSpecimenLiteQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var totalStopwatch = Stopwatch.StartNew();
        var specimenId = queryFilter.GetIntegerValue("SpecimenId");

        logWriter?.LogInfo(
            $"testlistforspecimenlite start specimenId={specimenId}",
            nameof(TestListForSpecimenLiteQueryHandler),
            nameof(RunAsync));

        var testRepository = _serviceProvider.GetService<ITestRepository>();
        var specialMappingFactory = _serviceProvider.GetService<ISpecialMappingFactory>();

        var sqlStopwatch = Stopwatch.StartNew();
        var list = await testRepository.GetTestListForSpecimenAsync(specimenId);
        sqlStopwatch.Stop();

        var tatStopwatch = Stopwatch.StartNew();
        await TurnAroundTimeEnricher.EnrichTestListAsync(_serviceProvider, list, isCultureTest: false);
        tatStopwatch.Stop();

        var mapperStopwatch = Stopwatch.StartNew();
        var json = JsonConvert.SerializeObject(list);
        var mapper = specialMappingFactory.GetMapper("testlistforspecimenliteresultmapper");
        var result = mapper.Map(json);
        mapperStopwatch.Stop();

        totalStopwatch.Stop();
        logWriter?.LogInfo(
            $"testlistforspecimenlite complete specimenId={specimenId} rowCount={list?.Count ?? 0} sqlMs={sqlStopwatch.ElapsedMilliseconds} tatMs={tatStopwatch.ElapsedMilliseconds} mapperMs={mapperStopwatch.ElapsedMilliseconds} totalMs={totalStopwatch.ElapsedMilliseconds}",
            nameof(TestListForSpecimenLiteQueryHandler),
            nameof(RunAsync));

        return result;
    }
}
