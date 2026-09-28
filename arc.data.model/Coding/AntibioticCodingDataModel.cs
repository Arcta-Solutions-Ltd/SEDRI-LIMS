namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the antibioticcoding table in the database.
    /// </summary>
    public class AntibioticCodingDataModel
    {
        /// <summary>
        /// Gets or sets the code that will be used for the antibiotic included in this group.
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the antibiotic table to the antibioticcoding table.
        /// </summary>
        public int AntibioticId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the antibioticcoding table. This links to the antibiotic coding list, in the listitem table.
        /// </summary>
        public int CodingId { get; set; }
    }
}
