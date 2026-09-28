namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Extension methods for grid column type IDs (list 109 subset used by gridfieldtypelist).
    /// Matching uses IDs only — never translated display values.
    /// </summary>
    public static class GridColumnTypeExtensions
    {
        /// <summary>Single-line text grid column type id.</summary>
        public const string SingleLine = "450";

        /// <summary>Combobox grid column type id.</summary>
        public const string ComboBox = "453";

        /// <summary>Dropdown grid column type id.</summary>
        public const string DropDown = "454";

        /// <summary>Toggle grid column type id.</summary>
        public const string Toggle = "457";

        /// <summary>Number grid column type id.</summary>
        public const string Number = "458";

        /// <summary>
        /// Returns true when the column type requires a list option (GridOption).
        /// </summary>
        /// <param name="gridTypeId">Grid column type id from gridfieldtypelist.</param>
        public static bool RequiresListOption(this string gridTypeId) =>
            gridTypeId == ComboBox || gridTypeId == DropDown;

        /// <summary>
        /// Returns true when MultiSelect is applicable for the column type.
        /// </summary>
        /// <param name="gridTypeId">Grid column type id from gridfieldtypelist.</param>
        public static bool SupportsMultiSelect(this string gridTypeId) =>
            gridTypeId == ComboBox || gridTypeId == DropDown;
    }
}
