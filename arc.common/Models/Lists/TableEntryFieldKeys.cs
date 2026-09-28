namespace arc.common.Models.Lists
{
    /// <summary>
    /// Key names used in the crafted contents of the Add and Edit Table Entry forms.
    /// Shared by the query handler that builds the form, the validator and the save events so that the
    /// load and save halves of the form cannot drift apart.
    /// </summary>
    public static class TableEntryFieldKeys
    {
        /// <summary>The entry text shown to the user.</summary>
        public const string Value = "Value";

        /// <summary>The enabled toggle, held as "Yes" or "No".</summary>
        public const string Enabled = "Enabled";

        /// <summary>The selected parent list item id(s), comma separated when a table allows several.</summary>
        public const string ParentId = "ParentId";

        /// <summary>Whether the entry is a system row, held as "Yes" or "No". Fixed rows take no parent.</summary>
        public const string Fixed = "Fixed";

        /// <summary>Name of the list supplying parent options when a table is parented by another table.</summary>
        public const string OptionName = "OptionName";

        /// <summary>Name of the list supplying parent options when a table parents itself.</summary>
        public const string InternalHierarchyParentOptionName = "InternalHierarchyParentOptionName";

        /// <summary>The id of the list (table) the entry belongs to.</summary>
        public const string ListId = "ListId";

        /// <summary>The id of the entry being edited, empty when adding.</summary>
        public const string Id = "Id";
    }
}
