using System.Collections.Generic;

namespace arc.common.Models.Config
{
    /// <summary>
    /// One row in the Page Order editor. Rows are matched by <see cref="Id"/> so that translated
    /// labels never take part in matching.
    /// </summary>
    public class PageOrderRowModel
    {
        /// <summary>Gets or sets the page name, used as the row id everywhere.</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets the page title shown to the user.</summary>
        public string Label { get; set; }

        /// <summary>Gets or sets the group this page belongs to, or null when ungrouped.</summary>
        public string GroupId { get; set; }

        /// <summary>Gets or sets the translation tag for the group heading.</summary>
        public string GroupTitle { get; set; }

        /// <summary>Gets or sets whether this page is a group anchor and cannot be moved.</summary>
        public bool Locked { get; set; }
    }

    /// <summary>
    /// Payload exchanged with the Page Order editor: the form being configured and its pages in
    /// their current order, annotated with group membership.
    /// </summary>
    public class PageOrderModel
    {
        /// <summary>Gets or sets the record identifier passed through from the configuration list.</summary>
        public string Id { get; set; }

        /// <summary>Gets or sets the pages in order.</summary>
        public List<PageOrderRowModel> PageOrder { get; set; }
    }
}
