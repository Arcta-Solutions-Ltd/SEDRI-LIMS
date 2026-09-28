using System;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Shared helpers for organisation enabled/disabled state stored in <c>organisation.enabled</c>.
    /// </summary>
    public static class OrganisationEnabledExtensions
    {
        /// <summary>
        /// Value stored when an organisation is active and selectable.
        /// </summary>
        public const string EnabledYes = "Yes";

        /// <summary>
        /// Value stored when an organisation is disabled and must not be selectable.
        /// </summary>
        public const string DisabledNo = "No";

        /// <summary>
        /// Returns a SQL predicate fragment that matches only enabled organisations.
        /// </summary>
        /// <param name="tableAlias">Optional table alias or name to qualify the column.</param>
        /// <returns>A predicate such as <c>enabled = 'Yes'</c> or <c>o.enabled = 'Yes'</c>.</returns>
        public static string EnabledOnlyPredicate(string tableAlias = null)
        {
            var column = string.IsNullOrWhiteSpace(tableAlias) ? "enabled" : $"{tableAlias}.enabled";
            return $"{column} = '{EnabledYes}'";
        }

        /// <summary>
        /// Determines whether the stored enabled flag represents an active organisation.
        /// </summary>
        /// <param name="enabled">The enabled flag value from the database.</param>
        /// <returns><c>true</c> when the organisation is enabled; otherwise <c>false</c>.</returns>
        public static bool IsOrganisationEnabled(this string enabled)
        {
            return string.Equals(enabled, EnabledYes, StringComparison.OrdinalIgnoreCase);
        }
    }
}
