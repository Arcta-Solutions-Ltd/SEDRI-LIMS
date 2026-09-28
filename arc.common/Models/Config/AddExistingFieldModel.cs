namespace arc.common.Models.Config
{
    /// <summary>
    /// Payload for the <c>addexistingfield</c> configuration event. Carries the field references the
    /// user selected in the Add Existing Field form. References are id based so the payload survives
    /// language translation of field labels and list items.
    /// </summary>
    public class AddExistingFieldModel
    {
        /// <summary>
        /// Gets or sets the target context id in the form
        /// <c>{formName}|{pageName}|{columnKey}|{formGroupKey}</c>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the comma separated field reference keys selected by the user. Each key has the
        /// form <c>{sourceFormName}|{sourcePageName}|{fieldId}</c>. All three parts are stable
        /// identifiers, never translated text.
        /// </summary>
        public string ExistingFieldIds { get; set; }
    }
}
