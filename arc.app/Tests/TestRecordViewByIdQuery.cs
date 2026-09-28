using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Special query that returns test record data (Id, TestName, TestResults, Status) for either
/// a culture test or direct test based on the source parameter.
/// </summary>
public class TestRecordViewByIdQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TestRecordViewByIdQuery> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestRecordViewByIdQuery"/> class.
    /// </summary>
    public TestRecordViewByIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<TestRecordViewByIdQuery>>();
    }

    /// <summary>
    /// Runs the query. Expects id and source parameters (source: 'culture' or 'direct').
    /// Returns JSON with Id, TestName, TestResults, Status.
    /// </summary>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var idParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
        var sourceParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "source");

        if (idParam == null || string.IsNullOrEmpty(idParam.Value))
        {
            _logger.LogWarning("TestRecordViewById: missing id parameter");
            return JsonConvert.SerializeObject(new object[] { });
        }

        if (!int.TryParse(idParam.Value, out var id))
        {
            _logger.LogWarning("TestRecordViewById: invalid id value {IdValue}", idParam.Value);
            return JsonConvert.SerializeObject(new object[] { });
        }

        var source = sourceParam?.Value?.ToLowerInvariant() ?? "direct";
        if (!string.IsNullOrWhiteSpace(sourceParam?.Value) && source != "culture" && source != "direct")
        {
            _logger.LogWarning(
                "TestRecordViewById: unexpected source {Source}, resolving as direct test path. TestId={TestId}",
                sourceParam!.Value,
                id);
        }

        var testRepository = _serviceProvider.GetRequiredService<ITestRepository>();

        Test test;
        if (source == "culture")
        {
            test = await testRepository.GetCultureTestAsync(id);
        }
        else
        {
            test = await testRepository.GetTestAsync(id);
        }

        if (test == null)
        {
            _logger.LogWarning("TestRecordViewById: no test row for TestId={TestId} Source={Source}", id, source);
            return JsonConvert.SerializeObject(new object[] { });
        }

        var result = new
        {
            Id = test.Id,
            TestName = test.TestName,
            TestResults = test.TestResults,
            Status = test.Status
        };

        return JsonConvert.SerializeObject(new[] { result });
    }
}
