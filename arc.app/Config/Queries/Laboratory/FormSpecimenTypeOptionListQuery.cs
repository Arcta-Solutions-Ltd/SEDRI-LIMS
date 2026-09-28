using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving the list of form specimen type options.
/// </summary>
internal class FormSpecimenTypeOptionListQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching form specimen type option data.
    /// </summary>
    /// <returns>
    /// A string representation of the query configuration, including the query name, type and translation
    /// settings.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'formspecimentypeoptionlistquery', 'Type': 'Config', 'Translate': true}";
    }
}
