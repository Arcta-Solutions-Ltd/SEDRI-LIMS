using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Provides the JSON definition for a report contents query.
/// </summary>
/// <remarks>
/// Implements <see cref="IDefinition"/> to supply the query payload
/// used when requesting report content from the backend.
/// </remarks>
internal class ReportContentsQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON string representing the report contents query.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing the "Query" and "Type" properties:
    /// { 'Query': 'reportcontentsquery', 'Type': 'Report' }
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'reportcontentsquery', 'Type': 'Report'}";
    }
}
