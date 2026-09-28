using System;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Helpers for direct and isolate test table names used in generic delete handling.
    /// </summary>
    public static class TestTableExtensions
    {
        /// <summary>
        /// Returns true when a delete with zero rows affected should be treated as success (already removed).
        /// </summary>
        /// <param name="tableName">Event config table name (e.g. Tests, CultureTests).</param>
        /// <returns>True for direct test and culture test tables.</returns>
        public static bool IsIdempotentTestDeleteTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }

            return tableName.Equals("Tests", StringComparison.OrdinalIgnoreCase)
                || tableName.Equals("CultureTests", StringComparison.OrdinalIgnoreCase);
        }
    }
}
