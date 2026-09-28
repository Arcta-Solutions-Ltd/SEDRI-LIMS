namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the alertlines table in the database.
    /// </summary>
    public class AlertLinesDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the alert table to the alertlines table.
        /// </summary>
        public int AlertId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the antibiotic table to the alertlines table.
        /// </summary>
        public string? AntibioticId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the breakpoint table. This links to the susceptibility list in the listitem table.
        /// </summary>
        public int SusceptibilityId { get; set; }
    }
}
