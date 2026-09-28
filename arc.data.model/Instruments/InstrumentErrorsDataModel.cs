namespace arc.data.model.Instruments
{
    /// <summary>
    /// Data model class for instrument errors.
    /// </summary>
    public class InstrumentErrorsDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string ProfileName { get; set; } = "";

        /// <summary>
        /// Gets or sets the instrument result ID.
        /// </summary>
        public int InstrumentResultId { get; set; }

        /// <summary>
        /// Gets or sets the instrument direction ID.
        /// </summary>
        public int InstrumentDirectionId { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the instrumenterrors table. This links to the InstrumentErrorStatus list (e.g. Failed=11, Resolved=12).
        /// </summary>
        public int ErrorStatusId { get; set; }

        /// <summary>
        /// Gets or sets the error text.
        /// </summary>
        public string? ErrorText { get; set; }

        /// <summary>
        /// Gets or sets the message in JSON format.
        /// </summary>
        [Jsonb]
        public string? Message { get; set; }
    }

}
