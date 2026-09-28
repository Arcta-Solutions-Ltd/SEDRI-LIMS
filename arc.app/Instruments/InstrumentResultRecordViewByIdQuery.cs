using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments;

/// <summary>
/// Special query for the instrument result record view: returns one row with all display fields by instrument result id.
/// </summary>
public class InstrumentResultRecordViewByIdQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    public InstrumentResultRecordViewByIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var idParam = queryFilter.Parameters?.FirstOrDefault(p => p.Key?.ToLower() == "id");
        if (idParam == null || string.IsNullOrEmpty(idParam.Value))
            return string.Empty;

        var repository = _serviceProvider.GetRequiredService<IInstrumentRepository>();
        var row = await repository.GetInstrumentResultRecordViewByIdAsync(queryFilter);
        if (row.Id == 0)
            return string.Empty;

        return JsonConvert.SerializeObject(row);
    }
}
