namespace arc.common.Models.Reports.ReportDesigner;

/// <summary>
/// Represents a basic field model with name and label properties.
/// This is a simplified field representation used in various contexts within the report designer.
/// </summary>
public class FieldModel
{
    /// <summary>
    /// Gets or sets the name of the field used for identification and reference.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the label text that will be displayed for this field.
    /// </summary>
    public string Label {  get; set; }
}
