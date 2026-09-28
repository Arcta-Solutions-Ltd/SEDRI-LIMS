using System;
using System.Linq;

namespace arc.data.Specimen;

/// <summary>
/// Turns the comma separated specimen id parameter used by the batch specimen queries into a distinct array of
/// positive ids, so each query does not have to repeat the parsing.
/// </summary>
internal static class SpecimenIdListParser
{
    /// <summary>
    /// Parses a comma separated list of specimen ids.
    /// </summary>
    /// <param name="value">Comma separated ids, which may be null or empty.</param>
    /// <returns>The distinct positive ids in the list, or an empty array when there are none.</returns>
    internal static int[] Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return [.. value
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(part => int.TryParse(part, out var id) ? id : 0)
            .Where(id => id > 0)
            .Distinct()];
    }
}
