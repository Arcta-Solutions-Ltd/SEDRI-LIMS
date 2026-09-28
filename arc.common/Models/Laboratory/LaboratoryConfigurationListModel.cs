using System.Collections.Generic;
using System.Linq;

namespace arc.common.Models.Laboratory;

/// <summary>
/// Represents a model containing a list of laboratory configurations.
/// </summary>
public class LaboratoryConfigurationListModel
{
    /// <summary>
    /// Gets or sets the list of laboratory configuration models.
    /// </summary>
    public List<LaboratoryConfigurationModel> LaboratoryList { get; set; }

    /// <summary>
    /// Retrieves configuration settings for the first entry in the laboratory list.
    /// </summary>
    /// <param name="configName">The name of the configuration to filter.</param>
    /// <returns>A collection of LaboratoryConfigsModel objects that match the given configuration name.</returns>
    public IEnumerable<LaboratoryConfigsModel> GetConfigurationsForFirstEntryInList(string configName)
    {
        return LaboratoryList.First().Configuration.Where(Item => Item.ConfigName == configName);
    }

    /// <summary>
    /// Retrieves the configuration model for a specified laboratory.
    /// </summary>
    /// <param name="laboratoryId">The unique identifier of the laboratory.</param>
    /// <returns>The LaboratoryConfigurationModel associated with the specified laboratory.</returns>
    public LaboratoryConfigurationModel GetConfigurationsForLaboratory(int laboratoryId)
    {
        return LaboratoryList.First(Item => Item.LaboratoryId == laboratoryId);
    }
}
