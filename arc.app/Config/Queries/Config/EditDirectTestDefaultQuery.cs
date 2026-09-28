using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Represents the default direct test edit query definition.
/// </summary>
internal class EditDirectTestDefaultQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for editing a direct test as a JSON-formatted string.
    /// </summary>
    /// <returns>A JSON string defining the query name and type.</returns>
    public string Get()
    {
        return @"{ 'Query': 'EditDirectTestDefaultQuery', 'Type': 'Config'}";
    }
}
