using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents a workflow list query definition that returns its configuration as a JSON string.
/// </summary>
internal class WorkflowListQuery : IDefinition
{
    /// <summary>
    /// Gets the JSON configuration for the workflow list query.
    /// </summary>
    /// <returns>
    /// A JSON string specifying the query name and type.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'WorkflowListQuery', 'Type': 'Special'}";
    }
}
