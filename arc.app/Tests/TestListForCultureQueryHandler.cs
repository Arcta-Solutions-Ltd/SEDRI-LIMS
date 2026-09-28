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
/// Special query handler for test list for culture. Fetches tests, enriches with TAT colour, then passes through TestListForSpecimenResultMapper for TestResults translation and TestDescription.
/// </summary>
public class TestListForCultureQueryHandler : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    public TestListForCultureQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();
        var totalStopwatch = Stopwatch.StartNew();
        var cultureId = queryFilter.GetIntegerValue("CultureId");
        logWriter?.LogInfo(
            $"testlistforculture start cultureId={cultureId}",
            nameof(TestListForCultureQueryHandler),
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
        var mapper = specialMappingFactory.GetMapper("testlistforcultureresultmapper");
        var result = mapper.Map(json);
        mapperStopwatch.Stop();

        totalStopwatch.Stop();
        logWriter?.LogInfo(
            $"testlistforculture complete cultureId={cultureId} rowCount={list?.Count ?? 0} sqlMs={sqlStopwatch.ElapsedMilliseconds} tatMs={tatStopwatch.ElapsedMilliseconds} mapperMs={mapperStopwatch.ElapsedMilliseconds} totalMs={totalStopwatch.ElapsedMilliseconds}",
            nameof(TestListForCultureQueryHandler),
            nameof(RunAsync));

        return result;
    }
}
