using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Defines the JSON payload for the SpecimenByIdForACK query used in acknowledgement processes.
/// </summary>
/// <remarks>
///   Query:     SpecimenByIdForACK  
///   TableName: Specimen  
///   Type:      Special  
///   Translate: true  
///   Tags:      SP  
/// </remarks>
internal class SpecimenByIdForACKQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON string that represents the SpecimenByIdForACK query definition.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing the query name, target table, type, translation flag, and tags.
    /// </returns>
    public string Get()
    {
        return @"
        {
            ""Query"":     ""SpecimenByIdForACK"",
            ""TableName"": ""Specimen"",
            ""Type"":      ""Special"",
            ""Translate"": true,
            ""Tags"":      ""SP""
        }";
    }
}
