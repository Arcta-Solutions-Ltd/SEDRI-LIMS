using arc.app.Config.Forms;
using arc.app.SystemConfig;
using arc.common.Models.Laboratory;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Configuration.Queries;

/// <summary>
/// Query for retrieving a single form specimen type option by its identifier. Kept separate from
/// <see cref="GetLaboratoryConfigByIdQuery"/> because the group here is a form name rather than a list item
/// id, so the description is resolved from the form configuration.
/// </summary>
/// <param name="serviceProvider">The service provider for resolving dependencies.</param>
internal class GetFormSpecimenTypeOptionByIdQuery(IServiceProvider serviceProvider) : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    /// <summary>
    /// Executes the query to retrieve a form specimen type option by its identifier.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration carrying the configuration id.</param>
    /// <param name="queryData">The query data configuration used for additional query context.</param>
    /// <returns>
    /// A serialized string representation of the configuration with the form title filled in.
    /// </returns>
    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var id = queryFilters.GetIntegerValue("id");
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var formAdapter = _serviceProvider.GetService<IFormConfigAdapter>();

        var laboratoryConfig = await laboratoryConfigRepository.GetByIdAsync<LaboratoryConfigsDataModel>("LaboratoryConfigs", id);
        var model = ArcJson.Deserialize<LaboratoryFormGroupingModel>(laboratoryConfig.Contents);

        var form = await formAdapter.GetFormAsync(model.GroupId);
        model.GroupDescription = form?.Title ?? model.GroupId;

        return ArcJson.Serialize(model);
    }
}
