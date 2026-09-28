namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// The parts of a report a section definition can belong to.
/// </summary>
/// <remarks>
/// The designer sends one of these on a section it has just created, because such a section has no
/// configs record yet and so no ConfigTypeId to carry. Keeping the values as names rather than type
/// numbers leaves the mapping from a scope to a ConfigTypeId in the save command, which is the only
/// place that has to know it.
/// </remarks>
public static class ConfigScope
{
    /// <summary>
    /// A section listed in one of the report's categories.
    /// </summary>
    public const string Section = "Section";

    /// <summary>
    /// A report header.
    /// </summary>
    public const string Header = "Header";

    /// <summary>
    /// A report footer.
    /// </summary>
    public const string Footer = "Footer";
}
