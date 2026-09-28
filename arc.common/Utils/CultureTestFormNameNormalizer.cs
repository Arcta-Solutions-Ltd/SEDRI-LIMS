using System;

namespace arc.common.Utils;

/// <summary>
/// Normalizes isolate test config names to the canonical form name suffix used in form definitions.
/// Ensures culture-type options, organism scope, AST, and selection lists match consistently.
/// </summary>
public static class CultureTestFormNameNormalizer
{
    /// <summary>
    /// Normalizes a culture test config name to lower-case with a "form" suffix when absent.
    /// </summary>
    /// <param name="name">Raw config name from laboratory config or form list.</param>
    /// <returns>Normalized form name, or empty string when input is null or whitespace.</returns>
    public static string Normalize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var lower = name.Trim().ToLowerInvariant();
        return lower.EndsWith("form", StringComparison.Ordinal) ? lower : lower + "form";
    }
}
