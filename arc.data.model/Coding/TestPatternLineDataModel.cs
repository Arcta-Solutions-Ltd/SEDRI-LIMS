namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the testpatternline table in the database.
    /// </summary>
    public class TestPatternLineDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the testpattern table to the testpatternline table.
        /// </summary>
        public int TestPatternId { get; set; }
        public int TestOrder {  get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the antibiotic table to the testpatternline table.
        /// </summary>
        public int AntibioticId { get; set; }
        /// <summary>
        /// Gets or sets the value of the dosage for the test pattern line.
        /// </summary>
        public string? Dosage { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the testpatternline table. This links to the test method list in the listitem table.
        /// </summary>
        public int TestMethodId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the testpatternline table. This links to the guideline list in the listitem table.
        /// </summary>
        public int GuidelinesId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the testpatternline table. This links to the test pattern category list in the listitem table.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Gets or sets whether this test pattern line should be included on a report by default.
        /// </summary>
        public bool PrintOnReport { get; set; }
    }
}
