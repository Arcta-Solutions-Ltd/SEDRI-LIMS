namespace arc.domain.Configuration.ReportsConfig;

public class ReportSectionFieldConfig
{
    public string Label { get; set; }
    public string Value { get; set; }
    public string Text { get; set; }
    public int Column { get; set; }
    public int Order { get; set; }
    public bool Image { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public string Format { get; set; }

    /// <summary>
    /// When true, the PDF renderer omits the value-cell border for this field.
    /// </summary>
    public bool NoBox { get; set; }
}
