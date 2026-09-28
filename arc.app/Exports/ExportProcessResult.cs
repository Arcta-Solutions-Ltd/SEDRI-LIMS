using System.Collections.Generic;

namespace arc.app.Exports
{
    /// <summary>
    /// Result of the export post-query processing step. Carries the processed pipe-delimited rows
    /// together with a set of column keys aligned one-to-one with the columns of each row.
    /// </summary>
    /// <remarks>
    /// The column keys let format writers (JSON/XML) resolve mapping attributes to columns by field id
    /// even when a single profile field expands into several columns (hierarchy, WHONET, comments).
    /// </remarks>
    public class ExportProcessResult
    {
        /// <summary>
        /// Gets or sets the processed rows. Index 0 is the header row; the remainder are data rows.
        /// </summary>
        public List<string> Lines { get; set; } = new();

        /// <summary>
        /// Gets or sets the field-id key for each output column, aligned with the columns of the header
        /// and data rows. Each key is the source field name (lower-cased); expanded fields repeat their
        /// key once per emitted column.
        /// </summary>
        public List<string> ColumnKeys { get; set; } = new();
    }
}
