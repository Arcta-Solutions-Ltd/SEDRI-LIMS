using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of specimen type culture type options.
/// </summary>
internal class SpecimenTypeCultureTypeOptionListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching specimen type culture type option data.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the query's name, type, and translation settings.
    /// It is designed to retrieve specimen type culture type option data for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata such as 
    /// the query name, type, and translation settings.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'specimentypeculturetypeoptionlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
