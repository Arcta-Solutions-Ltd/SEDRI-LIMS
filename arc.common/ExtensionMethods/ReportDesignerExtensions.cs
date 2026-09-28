using System;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Helpers for working with configuration names used by the report and workflow designers.
    /// The configs table stores names in lower case and treats them as stable identifiers, so
    /// every comparison and every write must go through these helpers rather than comparing raw strings.
    /// </summary>
    /// <remarks>
    /// The matching projections that convert designer models into the persisted arc.domain config shapes
    /// live in arc.domain/ExtensionMethods/ReportDesignerPersistenceExtensions.cs, because arc.common
    /// cannot reference arc.domain without creating a circular project reference.
    /// </remarks>
    public static class ReportDesignerExtensions
    {
        /// <summary>
        /// Normalises a configuration name to the form stored in the configs table: trimmed and lower case.
        /// </summary>
        /// <param name="name">The name to normalise.</param>
        /// <returns>The normalised name, or an empty string when the input is null or whitespace.</returns>
        public static string NormalisedConfigName(this string name)
        {
            return string.IsNullOrWhiteSpace(name) ? string.Empty : name.Trim().ToLowerInvariant();
        }

        /// <summary>
        /// Determines whether two configuration names identify the same configs record.
        /// </summary>
        /// <param name="left">The first name.</param>
        /// <param name="right">The second name.</param>
        /// <returns>True when both names normalise to the same non-empty value.</returns>
        public static bool IsSameConfigName(this string left, string right)
        {
            var normalisedLeft = left.NormalisedConfigName();
            var normalisedRight = right.NormalisedConfigName();

            return normalisedLeft.Length > 0
                   && string.Equals(normalisedLeft, normalisedRight, StringComparison.Ordinal);
        }
    }
}
