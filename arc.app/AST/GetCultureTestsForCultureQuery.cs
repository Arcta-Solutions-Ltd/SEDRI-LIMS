using arc.app.Common;
using arc.app.Specimen;
using arc.app.Tests;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.AST;

/// <summary>
/// Query that returns CultureTests (Id, TestName, Status) for a given culture.
/// Used when refetching after an isolate test save so the AST panel turns green.
/// </summary>
public class GetCultureTestsForCultureQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCultureTestsForCultureQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public GetCultureTestsForCultureQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query. Expects id or cultureid parameter. Returns JSON with CultureTests array.
    /// </summary>
    /// <param name="queryFilter">The filter configuration containing the culture identifier.</param>
    /// <param name="token">The token containing user or session context (optional).</param>
    /// <returns>A JSON-formatted string with CultureTests array (Id, TestName, Status).</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var logWriter = _serviceProvider.GetService<ILogWriter>();

        var cultureIdParam = queryFilter.Parameters?.FirstOrDefault(p => string.Equals(p.Key, "id", StringComparison.OrdinalIgnoreCase))
            ?? queryFilter.Parameters?.FirstOrDefault(p => string.Equals(p.Key, "cultureid", StringComparison.OrdinalIgnoreCase));

        if (cultureIdParam == null || string.IsNullOrEmpty(cultureIdParam.Value))
        {
            logWriter?.LogInfo("GetCultureTestsForCulture called without cultureId; returning empty CultureTests", nameof(GetCultureTestsForCultureQuery), nameof(RunAsync));
            return JsonConvert.SerializeObject(new { CultureTests = new List<object>() });
        }

        var cultureId = cultureIdParam.Value;
        logWriter?.LogInfo($"GetCultureTestsForCulture invoked for cultureId: {cultureId}", nameof(GetCultureTestsForCultureQuery), nameof(RunAsync));

        var cultureRepository = _serviceProvider.GetRequiredService<ICultureRepository>();
        var specimenRepository = _serviceProvider.GetRequiredService<ISpecimenRepository>();
        var testRepository = _serviceProvider.GetRequiredService<ITestRepository>();

        var cultureDetailsRetriever = new CultureDetailsRetriever(cultureRepository, specimenRepository, testRepository);
        var cultureDetails = await cultureDetailsRetriever.Get(cultureId);

        return JsonConvert.SerializeObject(new { CultureTests = cultureDetails.CultureTests });
    }
}
