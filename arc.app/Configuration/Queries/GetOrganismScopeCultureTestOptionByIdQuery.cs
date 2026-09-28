using arc.app.Coding;
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
/// Query for retrieving an organism scope culture test option configuration by ID.
/// Loads the config from laboratoryconfigs, deserializes organism scope fields,
/// resolves GroupDescription from taxonomy/organism group, and returns data for the edit form.
/// </summary>
internal class GetOrganismScopeCultureTestOptionByIdQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetOrganismScopeCultureTestOptionByIdQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    public GetOrganismScopeCultureTestOptionByIdQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the query to retrieve an organism scope culture test option by ID.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration containing the config ID.</param>
    /// <param name="queryData">Additional query data and parameters.</param>
    /// <returns>
    /// A serialized object with GroupDescription, AssociatedListId, and organism scope fields for the edit form.
    /// </returns>
    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var id = queryFilters.GetIntegerValue("id");
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var organismRepository = _serviceProvider.GetService<IOrganismRepository>();

        var laboratoryConfig = await laboratoryConfigRepository.GetByIdAsync<LaboratoryConfigsDataModel>("LaboratoryConfigs", id);
        var model = ArcJson.Deserialize<OrganismScopeCultureTestOptionModel>(laboratoryConfig.Contents);

        model.GroupDescription = await organismRepository.GetOrganismScopeDescriptionAsync(
            model.OrderId, model.FamilyId, model.GenusId, model.SpeciesId,
            model.SubspeciesId, model.SerotypeId, model.OrgGroupCodingId, model.OrganismId);

        return ArcJson.Serialize(model);
    }
}
