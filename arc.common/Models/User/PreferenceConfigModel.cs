namespace arc.common.Models.User
{
    public class PreferenceConfigModel
    {
        public string DataFullScreen { get; set; }
        public string ASTRowRemovalConfirmation { get; set; }

        /// <summary>
        /// Gets or sets the user's filter presets per view, as a JSON string of Dictionary&lt;string, List&lt;FilterPresetConfig&gt;&gt;.
        /// </summary>
        public string FilterPresets { get; set; }

        /// <summary>
        /// Gets or sets the user's column layout preferences per view, as a JSON string of Dictionary&lt;string, ColumnLayoutConfig&gt;.
        /// </summary>
        public string ColumnLayouts { get; set; }

        /// <summary>
        /// Gets or sets the user's home dashboard layout and section configuration as a JSON string (object stored in users.moredata.HomeDashboard).
        /// </summary>
        public string HomeDashboard { get; set; }
    }
}
