using System;
using System.Linq;
using arc.common.Constants;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Extension methods for permission-related string arrays.
/// </summary>
public static class PermissionArrayExtensions
{
    /// <summary>
    /// Returns an empty array when <paramref name="items"/> is null.
    /// </summary>
    /// <param name="items">The permission key array, which may be null.</param>
    /// <returns><paramref name="items"/> or an empty array when null.</returns>
    public static string[] OrEmpty(this string[]? items) => items ?? Array.Empty<string>();

    /// <summary>
    /// Returns a copy of <paramref name="items"/> with every mandatory sidebar key present.
    /// </summary>
    /// <param name="items">The allowed sidebar key array, which may be null.</param>
    /// <returns>An array containing all mandatory keys plus any existing keys.</returns>
    public static string[] EnsureMandatorySidebarItems(this string[]? items)
    {
        var list = items.OrEmpty().ToList();

        foreach (var mandatoryKey in MenuPermissionKeys.MandatorySidebarItems)
        {
            if (!list.Any(k => string.Equals(k, mandatoryKey, StringComparison.OrdinalIgnoreCase)))
            {
                list.Add(mandatoryKey);
            }
        }

        return list.ToArray();
    }
}
