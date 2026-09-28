using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Query that returns the culturetest ID for a given culture and test name.
/// Creates the culturetest record if it does not exist.
/// </summary>
public class GetOrCreateCultureTestIdQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrCreateCultureTestIdQuery"/> class.
    /// </summary>
    public GetOrCreateCultureTestIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query. Expects cultureId and testName parameters. Returns JSON with Id property.
    /// </summary>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var cultureIdParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "cultureid");
        var testNameParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "testname");
        if (cultureIdParam == null || string.IsNullOrEmpty(cultureIdParam.Value) || testNameParam == null || string.IsNullOrEmpty(testNameParam.Value))
            return JsonConvert.SerializeObject(new { Id = 0 });

        var cultureId = int.Parse(cultureIdParam.Value);
        var testName = testNameParam.Value;
        var testRepository = _serviceProvider.GetRequiredService<ITestRepository>();
        var id = await testRepository.GetOrCreateCultureTestIdAsync(cultureId, testName);
        return JsonConvert.SerializeObject(new { Id = id });
    }
}
