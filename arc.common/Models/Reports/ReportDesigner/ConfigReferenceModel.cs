namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Identifies a single configs record without carrying its contents.
/// Used by the designer to request a deletion, matching on the configs identity first
/// and only falling back to the configuration name when no identity is known.
/// </summary>
public class ConfigReferenceModel
{
    /// <summary>
    /// Gets or sets the configs table identity of the record, when the designer loaded one.
    /// </summary>
    public int? ConfigId { get; set; }

    /// <summary>
    /// Gets or sets the configs table type the record belongs to.
    /// </summary>
    /// <remarks>
    /// Both the identity lookup and the name fallback are scoped by type, so a deletion that names
    /// the wrong type finds nothing and silently does nothing.
    /// </remarks>
    public int? ConfigTypeId { get; set; }

    /// <summary>
    /// Gets or sets which part of the report the record belongs to, as one of the
    /// <see cref="ConfigScope"/> values. Used to resolve the type when <see cref="ConfigTypeId"/> is absent.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// Gets or sets the configuration name of the record. Used only when <see cref="ConfigId"/> is not supplied.
    /// </summary>
    public string Name { get; set; }
}
