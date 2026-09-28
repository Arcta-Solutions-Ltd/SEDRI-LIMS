using arc.common.Models.Reports.ReportDesigner;
using System.Collections.Generic;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Helpers for report line element font sizes stored in header, footer, and absolute section configuration.
/// </summary>
public static class ReportLineFontSizeExtensions
{
    /// <summary>Minimum allowed line FontSize in points.</summary>
    public const int Min = 6;

    /// <summary>Maximum allowed line FontSize in points.</summary>
    public const int Max = 20;

    /// <summary>Default font size when FontSize is omitted or zero at render time.</summary>
    public const int Default = 11;

    /// <summary>
    /// Clamps a line FontSize to the allowed range. Zero means "use default at render time" and is left unchanged.
    /// </summary>
    /// <param name="fontSize">Configured FontSize on a line element.</param>
    /// <returns>The clamped font size, or zero when absent.</returns>
    public static int ClampReportLineFontSize(this int fontSize)
    {
        if (fontSize == 0)
        {
            return 0;
        }

        if (fontSize < Min)
        {
            return Min;
        }

        if (fontSize > Max)
        {
            return Max;
        }

        return fontSize;
    }

    /// <summary>
    /// Clamps FontSize on every line in the collection and returns human-readable log messages for each change.
    /// </summary>
    /// <param name="lines">Line elements to normalize.</param>
    /// <param name="context">Description of the owning config record for log messages.</param>
    /// <returns>Information-level log messages describing clamped values.</returns>
    public static IReadOnlyList<string> NormalizeReportLineFontSizes(
        this IList<ReportLineModel> lines,
        string context)
    {
        var messages = new List<string>();

        if (lines == null || lines.Count == 0)
        {
            return messages;
        }

        foreach (var line in lines)
        {
            if (line == null)
            {
                continue;
            }

            var original = line.FontSize;
            var clamped = original.ClampReportLineFontSize();

            if (clamped == original)
            {
                continue;
            }

            line.FontSize = clamped;
            messages.Add(
                $"Clamped line FontSize from {original} to {clamped} on {context} element Line={line.Line} Left={line.Left}");
        }

        return messages;
    }
}
