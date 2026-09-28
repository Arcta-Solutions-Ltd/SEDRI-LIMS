using arc.app.Common;
using arc.app.Config;
using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Configuration.WorkflowsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Configuration;

/// <summary>
/// Provides functionality to determine the current workflow configuration for a specimen.
/// Implements IWorkflowFinder for workflow retrieval based on specimen details.
/// </summary>
public class WorkflowFinder : IWorkflowFinder
{
    private readonly ISpecimenRepository _specimenRepository;
    private readonly IConfigListUtils _configListUtils;
    private readonly IConfigRepository _configRepository;
 
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowFinder"/> class with required dependencies.
    /// </summary>
    /// <param name="specimenRepository">Repository for accessing specimen data.</param>
    /// <param name="configListUtils">Utility for retrieving configuration lists.</param>
    /// <param name="configRepository">Repository for accessing configuration records.</param>
    public WorkflowFinder(ISpecimenRepository specimenRepository, IConfigListUtils configListUtils, IConfigRepository configRepository)
    {
        _specimenRepository = specimenRepository;
        _configListUtils = configListUtils;
        _configRepository = configRepository;
    }

    /// <summary>
    /// Retrieves the current workflow configuration for a given specimen ID.
    /// </summary>
    /// <param name="specimenId">The specimen's unique identifier.</param>
    /// <param name="token">Authentication token for accessing configurations.</param>
    /// <returns>
    /// A <see cref="WorkflowConfig"/> representing the workflow configuration for the specimen.
    /// </returns>
    public async Task<WorkflowConfig> GetCurrentWorkflowAsync(int specimenId, int specimenTypeId, int laboratoryId, TokenInfoModel token)
    {
        laboratoryId = laboratoryId == 0 && ! string.IsNullOrEmpty(token.LaboratoryId) ? int.Parse(token.LaboratoryId) : laboratoryId;    
        var queryFilter = new QueryFilterConfig();
        if (specimenId > 0)
        {
            queryFilter.AddInteger("id", specimenId);
            var specimenRecord = await _specimenRepository.SpecimenByIdAsync(queryFilter);
            specimenTypeId = specimenRecord.SpecimenTypeId;
            laboratoryId = specimenRecord.LaboratoryId;
        }

        var laboratoryConfigurationList = await _configListUtils.GetLaboratoryConfigAsync(token);
        var currentLaboratory = laboratoryConfigurationList.GetConfigurationsForLaboratory(laboratoryId);

        var workflowId = currentLaboratory.DefaultWorkflowId;
        foreach (var config in currentLaboratory.Configuration)
        {
            if (config.ConfigName.IsSameAs("specimentypeworkflow") && config.AssociatedListId.CsvContains(specimenTypeId))
            {
                workflowId = int.Parse(config.GroupId);
            }
        }

        queryFilter = new QueryFilterConfig().AddInteger("id", workflowId);
        var configRecord = await _configRepository.SingleConfigByIdAsync(queryFilter);
        return JsonConvert.DeserializeObject<WorkflowConfig>(configRecord.Contents);
    }
}
