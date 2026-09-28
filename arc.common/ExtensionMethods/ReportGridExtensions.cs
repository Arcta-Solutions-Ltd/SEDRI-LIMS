using System;
using System.Linq;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Helpers for matching report grids to one another and for reading a format grid's column count.
    /// </summary>
    /// <remarks>
    /// A grid is identified by an id in three places that have to line up: the form fieldgrid id
    /// (for example crystalgrid), the data section grid Name (CrystalGrid) and the report section
    /// grid binding Name. Those ids are stable across languages, unlike a grid's Description or the
    /// fieldgrid Label, which are language catalogue tokens. Every comparison must therefore go
    /// through these helpers rather than comparing display text.
    /// </remarks>
    public static class ReportGridExtensions
    {
        /// <summary>
        /// Normalises a grid identifier so that the form, data section and section binding spellings
        /// of the same grid compare equal: trimmed, spaces removed and lower case.
        /// </summary>
        /// <param name="gridName">The grid identifier to normalise.</param>
        /// <returns>The normalised identifier, or an empty string when the input is null or whitespace.</returns>
        public static string NormalisedGridId(this string gridName)
        {
            return string.IsNullOrWhiteSpace(gridName)
                ? string.Empty
                : gridName.Trim().Replace(" ", string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// Determines whether two grid identifiers refer to the same grid.
        /// </summary>
        /// <param name="gridName">The first identifier.</param>
        /// <param name="other">The second identifier.</param>
        /// <returns>True when both identifiers normalise to the same non-empty value.</returns>
        public static bool IsSameGridId(this string gridName, string other)
        {
            var normalisedLeft = gridName.NormalisedGridId();
            var normalisedRight = other.NormalisedGridId();

            return normalisedLeft.Length > 0
                   && string.Equals(normalisedLeft, normalisedRight, StringComparison.Ordinal);
        }

        /// <summary>
        /// Counts the columns a format grid defines from its pipe delimited width definition.
        /// </summary>
        /// <param name="widthDefinition">A pipe delimited width string such as "150|150|80".</param>
        /// <returns>The number of column widths, or zero when the definition is null or empty.</returns>
        public static int GridColumnCount(this string widthDefinition)
        {
            return string.IsNullOrWhiteSpace(widthDefinition)
                ? 0
                : widthDefinition.Split('|', StringSplitOptions.RemoveEmptyEntries).Count(w => !string.IsNullOrWhiteSpace(w));
        }
    }
}
