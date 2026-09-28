using arc.app.Config.Forms;
using arc.common.ExtensionMethods;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;

namespace arc.app.Configuration.Queries;

internal class GetAddPageQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;

    internal GetAddPageQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Loads initial data for the Add Page form, including whether TableName selection applies.
    /// </summary>
    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var id = queryFilters.Parameters.First(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase)).Value;
        var formName = id.Split("|").Last();

        var formAdapter = _serviceProvider.GetRequiredService<IFormConfigAdapter>();
        var form = await formAdapter.GetFormAsync(formName);
        var showTableName = FormPageTargetTableExtensions.IsSpecimenRecordForm(form?.SingleItemName);

        var result = new
        {
            Id = id,
            ShowTableName = showTableName ? "Yes" : "No",
            TableName = showTableName ? "specimen" : string.Empty
        };

        return JsonConvert.SerializeObject(result);
    }
}
