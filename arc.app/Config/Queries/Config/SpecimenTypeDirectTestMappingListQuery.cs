using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the specimen type to direct test mapping list.
/// </summary>
internal class SpecimenTypeDirectTestMappingListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for mapping specimen types to direct tests.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the query name, type, and translation settings 
    /// required to fetch the mapping list for specimen types and direct tests.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata and query details.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'SpecimenTypeDirectTestMappingListQuery', 'Type': 'Config', 'Translate': true}";
    }
}
