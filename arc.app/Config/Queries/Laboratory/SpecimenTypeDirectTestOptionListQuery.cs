using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of specimen type direct test options.
/// </summary>
internal class SpecimenTypeDirectTestOptionListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching specimen type direct test option data.
    /// </summary>
    /// <remarks>
    /// This configuration defines the query's name, type, and translation settings.
    /// It is used to retrieve specimen type direct test option data for configuration purposes.
    /// </remarks>
    /// <returns>
    /// A string representation of the query configuration, including metadata such as 
    /// the query name, type, and translation settings.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'specimentypedirecttestoptionlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
