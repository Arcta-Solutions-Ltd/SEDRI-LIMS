using arc.app.Common;

namespace arc.app.Config.Reports.Footers;

/// <summary>
/// Factory class to create instances of report footers based on the definition name.
/// </summary>
internal class ReportFooterFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a report footer based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition for the report footer.</param>
    /// <returns>An instance of a report footer that implements IDefinition, or null if the definition name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "defaultspecimenreportfooter" => new DefaultSpecimenReportFooter(),
            _ => null,
        };
    }
}
