using arc.app.SystemConfig;
using arc.common.Utils;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries;
internal class GetEditMappingQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;

    internal GetEditMappingQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var configRepository = _serviceProvider.GetService<IConfigRepository>();
        var jsonReplacer = _serviceProvider.GetService<IJsonReplacer>();

        var result = await configRepository.SingleConfigByIdAsync(queryFilters);

        var contents = jsonReplacer.AddNewStringValue(result.Contents, "configname", result.ConfigName);

        return contents;
    }
}
