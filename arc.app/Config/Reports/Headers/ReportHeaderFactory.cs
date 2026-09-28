using arc.app.Common;

namespace arc.app.Config.Reports.Headers;

/// <summary>
/// Factory class to create report header definitions based on the provided definition name.
/// </summary>
internal class ReportHeaderFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a report header definition based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>An instance of the corresponding report header definition, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "defaultspecimenreportheader" => new DefaultSpecimenReportHeader(),
            _ => null,
        };
    }
}
