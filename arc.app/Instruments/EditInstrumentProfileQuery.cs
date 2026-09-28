using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace arc.app.Instruments;

/// <summary>
/// Represents a query for editing instrument profiles.
/// </summary>
internal class EditInstrumentProfileQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditInstrumentProfileQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    internal EditInstrumentProfileQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Asynchronously loads one instrument profile for the edit wizard (step 1 initial data via <see cref="ISingleInstrumentProfile.GetAsync"/>).
    /// </summary>
    /// <param name="queryFilter">The query filter configuration.</param>
    /// <param name="token">The token information model.</param>
    /// <returns>A task representing the asynchronous operation, with a string result.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var singleInstrumentProfile = _serviceProvider.GetService<ISingleInstrumentProfile>();

        var returnValue = await singleInstrumentProfile.GetAsync(queryFilter.Parameters[0].Value);

        return JsonConvert.SerializeObject(returnValue);
    }
}

