using arc.common.Models.Reports.ReportDesigner;
using Newtonsoft.Json.Linq;

namespace arc.common.Models.Config;

/// <summary>
/// What the workflow designer sends when the user saves. The designer only edits the workflow
/// document, so the whole document comes back and replaces the stored contents of one configs row.
/// </summary>
/// <remarks>
/// The record to update is chosen by <see cref="ConfigId"/>, never by name, so renaming a workflow
/// cannot cause the save to create a second record or overwrite an unrelated one.
/// </remarks>
public class SaveWorkflowDesignerConfigModel
{
    /// <summary>
    /// Gets or sets the configs table identity of the workflow being saved. Required.
    /// </summary>
    public int ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the configuration name of the workflow, used for logging and to confirm the
    /// identity resolves to the record the designer thinks it loaded.
    /// </summary>
    public string ConfigName { get; set; }

    /// <summary>
    /// Gets or sets the edited workflow document, in the same shape it was loaded in.
    /// </summary>
    public JObject Workflow { get; set; }
}

/// <summary>
/// Result returned to the workflow designer after a save.
/// </summary>
public class WorkflowDesignerSaveResultModel
{
    /// <summary>
    /// Gets or sets the outcome of writing the workflow record.
    /// </summary>
    public ConfigSaveResultModel Workflow { get; set; }

    /// <summary>
    /// Gets or sets the number of steps in the document that was stored, so the caller can log or
    /// assert that the save was not a silent truncation.
    /// </summary>
    public int StepCount { get; set; }
}
