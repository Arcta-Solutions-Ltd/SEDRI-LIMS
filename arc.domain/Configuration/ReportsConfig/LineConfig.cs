namespace arc.domain.Configuration.ReportsConfig;

/// <summary>
/// Represents the configuration for a line in a report.
/// </summary>
public class LineConfig
{
    /// <summary>
    /// Line number
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Left margin in pixels
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Name of field to render
    /// </summary>
    public string Field { get; set; }

    /// <summary>
    /// Text to print
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Font size in points
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// Whether font should print in bold
    /// </summary>
    public bool Bold { get; set; }

    /// <summary>
    /// Name of function to call to calculate value.
    /// Currently accepts "Pages" and "PrintedDate"
    /// </summary>
    public string Calc { get; set; }
}
