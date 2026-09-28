using arc.common.Models.Reports;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for report section HeadingText and format heading geometry.
/// </summary>
public static class ReportSectionHeadingExtensions
{
    /// <summary>
    /// Returns true when HeadingText should be rendered as a section heading on print.
    /// Whitespace-only and empty strings mean no heading.
    /// </summary>
    /// <param name="headingText">The section HeadingText value from configuration.</param>
    /// <returns>True when a section heading should appear on the printed report.</returns>
    public static bool HasSectionHeading(this string headingText) =>
        !string.IsNullOrWhiteSpace(headingText);

    /// <summary>
    /// Normalizes HeadingText for persistence: null/whitespace becomes empty string.
    /// </summary>
    /// <param name="headingText">The section HeadingText value to normalize.</param>
    /// <returns>Trimmed heading text, or an empty string when absent or whitespace-only.</returns>
    public static string NormalizeSectionHeading(this string headingText) =>
        string.IsNullOrWhiteSpace(headingText) ? string.Empty : headingText.Trim();

    /// <summary>
    /// True when the format defines a heading slot (non-empty Heading array).
    /// </summary>
    /// <param name="formatHeadingCount">Number of heading lines on the format, if known.</param>
    /// <returns>True when format heading geometry should be applied.</returns>
    public static bool FormatHeadingEnabled(int formatHeadingCount) =>
        formatHeadingCount > 0;

    /// <summary>
    /// Builds the printable heading line: text from section HeadingText,
    /// geometry from format Heading when enabled, otherwise defaults.
    /// Returns null when HeadingText is empty.
    /// </summary>
    /// <param name="headingText">Section HeadingText supplying the heading content.</param>
    /// <param name="formatHeadingEnabled">Whether the format heading slot is enabled.</param>
    /// <param name="formatLine">Format line number, when enabled.</param>
    /// <param name="formatLeft">Format left position, when enabled.</param>
    /// <param name="formatFontSize">Format font size, when enabled.</param>
    /// <param name="formatBold">Format bold flag, when enabled.</param>
    /// <returns>A merged heading line, or null when there is no section heading text.</returns>
    public static PrintHeadingLineModel BuildPrintHeadingLine(
        this string headingText,
        bool formatHeadingEnabled,
        int formatLine,
        int formatLeft,
        int formatFontSize,
        bool formatBold)
    {
        var text = headingText.NormalizeSectionHeading();
        if (!text.HasSectionHeading())
        {
            return null;
        }

        return new PrintHeadingLineModel
        {
            Line = formatHeadingEnabled && formatLine > 0 ? formatLine : 1,
            Left = formatHeadingEnabled && formatLeft > 0 ? formatLeft : 20,
            Text = text,
            FontSize = formatHeadingEnabled && formatFontSize > 0 ? formatFontSize : 14,
            Bold = formatHeadingEnabled ? formatBold : true
        };
    }
}
