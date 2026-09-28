using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the query definition for editing a specimen type workflow configuration.
/// </summary>
internal class EditSpecimenTypeWorkflowQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string containing the query name and type.</returns>
    public string Get()
    {
        return @"{ 'Query': 'EditSpecimenTypeWorkflowQuery', 'Type': 'Config'}";
    }
}
