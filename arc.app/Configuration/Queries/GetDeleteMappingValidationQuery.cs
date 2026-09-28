using arc.app.Common;
using arc.app.Exports;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries;
internal class GetDeleteMappingValidationQuery : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    internal GetDeleteMappingValidationQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token = null)
    {
        var exportProfileFieldRepository = _serviceProvider.GetService<IExportProfileFieldRepository>();

        var mappings = await exportProfileFieldRepository.GetAllFieldMappingsAsync(queryFilter);

        var res = "0";

        if(mappings != null)
        {
            res = mappings.Count(p => p.Equals(queryFilter.GetStringValue("configname"), StringComparison.Ordinal)).ToString();
        }

        return res;
    }
}
