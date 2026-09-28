namespace arc.common.Models.SystemConfig;

/// <summary>
/// Represents a data model for a workflow, encapsulating its unique identifier,
/// name, and description. This model is typically used to transfer workflow data
/// between different layers of an application.
/// </summary>
public class WorkflowListModel
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the description of the workflow.
    /// </summary>
    public string Description { get; set; }
}
