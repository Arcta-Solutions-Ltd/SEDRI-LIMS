using System.Collections.Generic;

namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Result returned to the report designer after a save, describing exactly what was written to the
/// configs table. The designer uses it to reconcile any backend rename before the configuration is refetched.
/// </summary>
public class ReportDesignerSaveResultModel
{
    /// <summary>
    /// Gets or sets the configuration name the report was saved under.
    /// </summary>
    public string ReportName { get; set; }

    /// <summary>
    /// Gets or sets the outcome for each custom format in the change set.
    /// </summary>
    public List<ConfigSaveResultModel> Formats { get; set; } = [];

    /// <summary>
    /// Gets or sets the outcome for each section definition in the change set.
    /// </summary>
    public List<ConfigSaveResultModel> Sections { get; set; } = [];

    /// <summary>
    /// Gets or sets the outcome for the report record itself.
    /// </summary>
    public ConfigSaveResultModel Report { get; set; }
}

/// <summary>
/// Describes the outcome of writing a single configs record.
/// </summary>
public class ConfigSaveResultModel
{
    /// <summary>
    /// Gets or sets the configs table identity of the record after the write.
    /// </summary>
    public int ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the configuration type the record was written under.
    /// </summary>
    public int ConfigTypeId { get; set; }

    /// <summary>
    /// Gets or sets the name the designer asked for.
    /// </summary>
    public string RequestedName { get; set; }

    /// <summary>
    /// Gets or sets the name actually stored. Differs from <see cref="RequestedName"/> when a
    /// new record collided with an existing configuration name and had to be renamed.
    /// </summary>
    public string SavedName { get; set; }

    /// <summary>
    /// Gets or sets the change the designer requested for this record.
    /// </summary>
    public ConfigChangeState State { get; set; }

    /// <summary>
    /// Gets or sets what actually happened: Inserted, Updated, Deleted, NotFound or Skipped.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the reason the record was skipped, when <see cref="Action"/> is Skipped.
    /// </summary>
    public string SkipReason { get; set; }
}
