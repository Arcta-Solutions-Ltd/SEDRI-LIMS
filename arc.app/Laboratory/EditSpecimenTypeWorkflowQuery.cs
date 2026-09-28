using arc.app.Common;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common.Models.Laboratory;
using arc.common.Models.SystemConfig;
using arc.common.Utils;
using arc.data.model.Configuration;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace arc.app.Laboratory;
internal class EditSpecimenTypeWorkflowQuery : ISingleConfig
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLaboratoryConfigByIdQuery"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for resolving dependencies.</param>
    public EditSpecimenTypeWorkflowQuery(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the query to retrieve a laboratory configuration by its identifier.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration containing the necessary filter parameters, such as the configuration ID.</param>
    /// <param name="queryData">The query data configuration used for additional query context.</param>
    /// <returns>
    /// A serialized string representation of the laboratory configuration, including its deserialized and updated model.
    /// </returns>
    public async Task<string> GetAsync(QueryFilterConfig queryFilters, QueryConfig queryData)
    {
        var id = queryFilters.GetIntegerValue("id");
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var laboratoryConfig = await laboratoryConfigRepository.GetByIdAsync<LaboratoryConfigsDataModel>("LaboratoryConfigs", id);
        var model = ArcJson.Deserialize<LaboratoryListGroupingModel>(laboratoryConfig.Contents);

        var configRepository = _serviceProvider.GetService<IConfigRepository>();
        var queryFilter = new QueryFilterConfig().AddInteger("id", model.GroupId);
        var configRecord = await configRepository.SingleConfigByIdAsync(queryFilter);
        var record = ArcJson.Deserialize<WorkflowListModel>(configRecord.Contents);
        model.GroupDescription = record.Description;

        return ArcJson.Serialize(model);
    }
}

