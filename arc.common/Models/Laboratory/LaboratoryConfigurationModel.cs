using System.Collections.Generic;
using System.Linq;

namespace arc.common.Models.Laboratory;

/// <summary>
/// Represents the configuration model for a laboratory.
/// This class stores configuration settings, along with identifiers for the laboratory and its associated organization.
/// </summary>
public class LaboratoryConfigurationModel
{
    /// <summary>
    /// A list containing configuration details for the laboratory.
    /// Each entry defines specific settings or attributes relevant to the lab environment.
    /// </summary>
    public List<LaboratoryConfigsModel> Configuration { get; set; }

    /// <summary>
    /// Unique identifier for the laboratory.
    /// This helps distinguish different laboratories within the system.
    /// </summary>
    public int LaboratoryId { get; set; }

    /// <summary>
    /// Represents the default workflow identifier.
    /// This property stores the unique ID of the default workflow associated with an entity.
    /// </summary>
    public int DefaultWorkflowId { get; set; }


    /// <summary>
    /// Sets whether reports for a laboratory need to be approved laboratory.
    /// </summary>
    public string ApproveReports { get; set; } = "Yes";

    /// <summary>
    /// When 'Yes', manual susceptibility changes on AST require audit reason entry.
    /// </summary>
    public string RecordSusceptibilityChangeAudit { get; set; } = "No";

    /// <summary>
    /// Retrieves configuration settings for a given configuration name.
    /// </summary>
    /// <param name="configName">The name of the configuration to filter.</param>
    /// <returns>A collection of LaboratoryConfigsModel objects that match the given configuration name.</returns>
    public IEnumerable<LaboratoryConfigsModel> GetConfigurationSetting(string configName)
    {
        return Configuration.Where(item => item.ConfigName == configName);
    }
}


