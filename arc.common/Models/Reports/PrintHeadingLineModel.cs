namespace arc.common.Models.Reports;

/// <summary>
/// A merged printable section heading line: text from section HeadingText,
/// geometry from the format heading slot when enabled.
/// </summary>
public class PrintHeadingLineModel
{
    /// <summary>
    /// Line number within the section heading block.
    /// </summary>
    public int Line { get; set; }

    /// <summary>
    /// Left margin in pixels.
    /// </summary>
    public int Left { get; set; }

    /// <summary>
    /// Heading text from section HeadingText.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Font size in pixels.
    /// </summary>
    public int FontSize { get; set; }

    /// <summary>
    /// Whether the heading renders in bold.
    /// </summary>
    public bool Bold { get; set; }
}
