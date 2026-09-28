using arc.app.Common;
using arc.common.Models;
using arc.common.Utils;
using arc.data.model.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Instruments;
/// <summary>
/// Represents a query for viewing instrument error details.
/// </summary>
internal class ViewInstrumentErrorDetailsQuery : IQueryRun
{
    /// <summary>
    /// Provides access to services required by the query.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewInstrumentErrorDetailsQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    public ViewInstrumentErrorDetailsQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query asynchronously to retrieve instrument error details.
    /// </summary>
    /// <param name="queryFilter">The filter configuration for the query.</param>
    /// <param name="token">Optional token information for the query.</param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, containing the serialized instrument error details.
    /// </returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var generalRepository = _serviceProvider.GetRequiredService<IGeneralRepository>();

        var instrumentRecord = await generalRepository.GetSingleRecordAsync(new InstrumentErrorsDataModel { Id = queryFilter.GetIntegerValue("id") }, "instrumenterrors", "Id");

        var response = new { instrumentRecord.Id, instrumentRecord.Message };
        return ArcJson.Serialize(response);
    }
}

