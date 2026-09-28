namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a single section source entry on a report, pairing the category key stored in the
/// report configuration with the data source that supplies it.
/// </summary>
public class SectionSourceModel
{
    /// <summary>
    /// Gets or sets the section source name (for example "MainSections", "OrganismSections", "FinalSections").
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the source that supplies the sections (for example "Main" or "Organism").
    /// </summary>
    public string Source { get; set; }
}
