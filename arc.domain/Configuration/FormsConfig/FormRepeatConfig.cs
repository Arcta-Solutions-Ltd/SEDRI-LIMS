namespace arc.domain.Configuration.FormsConfig
{
    /// <summary>
    /// Lets a form offer to be run again from part way through once it has saved, so a user can register
    /// several specimens against the same patient, admission and request without re-entering the shared
    /// pages. Absent on a form that saves once and closes.
    /// </summary>
    public class FormRepeatConfig
    {
        /// <summary>
        /// Name of the page the repeated run starts at. Values entered on this page and any page after it
        /// are cleared; earlier pages keep what was entered. Cleared fields receive config-driven defaults
        /// on the portal via GetDefaultValuesFromPage (same pipeline as initial form open).
        /// </summary>
        public string FromPage { get; set; }

        /// <summary>Language key of the question asked after a successful save.</summary>
        public string Prompt { get; set; }

        /// <summary>Language key of the title on the prompt.</summary>
        public string Title { get; set; }
    }
}
