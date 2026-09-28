using System.Collections.Generic;

namespace arc.common.Models.User
{
    /// <summary>
    /// User-specific column layout preferences for a list view.
    /// Stored per view in Users.MoreData.ColumnLayouts[viewName].
    /// </summary>
    public class ColumnLayoutConfig
    {
        /// <summary>
        /// Column keys to display. When null or empty, all config columns are shown.
        /// </summary>
        public List<string> VisibleKeys { get; set; }

        /// <summary>
        /// Display order of column keys. When null or empty, config order is used.
        /// </summary>
        public List<string> Order { get; set; }

        /// <summary>
        /// Resized column widths in pixels, keyed by column key.
        /// </summary>
        public Dictionary<string, int> Widths { get; set; }
    }
}
