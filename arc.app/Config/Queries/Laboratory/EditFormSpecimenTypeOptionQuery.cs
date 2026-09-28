using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query for retrieving a single form specimen type option so it can be edited or deleted.
/// </summary>
internal class EditFormSpecimenTypeOptionQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query configuration for fetching one form specimen type option by id.
    /// </summary>
    /// <returns>
    /// A string representation of the query configuration, including the query name and type.
    /// </returns>
    public string Get()
    {
        return @"{ 'Query': 'editformspecimentypeoptionquery', 'Type': 'Config'}";
    }
}
