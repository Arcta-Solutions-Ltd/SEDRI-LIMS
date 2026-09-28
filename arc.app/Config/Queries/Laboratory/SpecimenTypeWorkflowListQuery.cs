using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Defines a query for fetching the list of specimen type workflows configuration.
/// </summary>
internal class SpecimenTypeWorkflowListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the specimen type workflow list query configuration as a JSON-formatted string.
    /// </summary>
    /// <returns>
    /// A JSON string containing the query identifier, its type, and a flag indicating whether translation is required.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'specimentypeworkflowlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
