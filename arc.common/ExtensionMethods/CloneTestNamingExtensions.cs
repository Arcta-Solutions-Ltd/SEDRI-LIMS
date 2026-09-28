using arc.common.Models.Config;
using System;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Derives stable config names for a cloned test from the reserved form name
/// returned by <c>GetNextAvailableNameAsync</c> (always ends with <c>form</c>).
/// </summary>
public static class CloneTestNamingExtensions
{
    private const string FormSuffix = "form";

    /// <summary>
    /// Builds canonical clone artifact names from the reserved configs-table form name.
    /// </summary>
    /// <param name="reservedFormName">
    /// The name returned by <c>GetNextAvailableNameAsync</c> with suffix <c>form</c>
    /// (for example <c>esblgridcloneform</c>).
    /// </param>
    /// <returns>Derived names for form, save event, data section, report section, and UI event.</returns>
    public static CloneTestNames DeriveFromReservedFormName(string reservedFormName)
    {
        var normalized = string.IsNullOrWhiteSpace(reservedFormName)
            ? string.Empty
            : reservedFormName.Trim().ToLowerInvariant();

        var saveEventName = normalized.EndsWith(FormSuffix, StringComparison.OrdinalIgnoreCase)
            ? normalized[..^FormSuffix.Length]
            : normalized;

        return new CloneTestNames
        {
            FormConfigName = normalized,
            SaveEventName = saveEventName,
            DataSectionName = normalized + "datasection",
            ReportSectionName = normalized + "section",
            UIEventName = normalized + "uievent",
        };
    }
}
