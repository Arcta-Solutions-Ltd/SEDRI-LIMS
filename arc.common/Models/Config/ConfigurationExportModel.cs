namespace arc.common.Models.Config
{
    /// <summary>
    /// Model that represents the options on the export configuration screen.
    /// </summary>
    public class ConfigurationExportModel
    {
        /// <summary>
        /// Gets or sets whether configuration data should be exported.
        /// </summary>
        public string Configuration;
        /// <summary>
        /// Gets or sets whether list data should be exported.
        /// </summary>
        public string ListItems;
        /// <summary>
        /// Gets or sets whether language data should be exported.
        /// </summary>
        public string Language;
        /// <summary>
        /// Gets or sets whether coding data should be exported.
        /// </summary>
        public string Coding;
        /// <summary>
        /// Gets or sets whether quality control data should be exported.
        /// </summary>
        public string QualityControl;
        /// <summary>
        /// Gets or sets whether export profile data should be exported.
        /// </summary>
        public string Exportprofile;
    }
}
