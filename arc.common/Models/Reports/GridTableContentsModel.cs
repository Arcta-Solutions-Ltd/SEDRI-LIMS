using System.Collections.Generic;

namespace arc.common.Models.Reports
{
    /// <summary>
    /// The rows a test contributes to one report table, gathered under the data key the report
    /// section's grid binding renders.
    /// </summary>
    /// <remarks>
    /// A data section usually declares one grid per form fieldgrid, so each key has a single source
    /// and the rows are written as entered. Where a data section declares fewer grids than the form
    /// has fieldgrids, several sources share a key; the rows are then prefixed with the source grid's
    /// label so the report can split them into headed sub tables.
    /// </remarks>
    public class GridTableContentsModel
    {
        /// <summary>
        /// The data key of the data section grid these rows belong to, matching ContentsConfig.Data
        /// on the translated report section.
        /// </summary>
        public string DataKey { get; set; }

        /// <summary>
        /// The form fieldgrid ids whose rows were gathered under this key.
        /// </summary>
        public List<string> SourceGridIds { get; set; } = new List<string>();

        /// <summary>
        /// The pipe separated rows to render, in the order the sources were read.
        /// </summary>
        public List<string> Rows { get; set; } = new List<string>();
    }
}
