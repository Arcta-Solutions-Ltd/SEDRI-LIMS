namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the organismcoding table in the database.
    /// </summary>
    public class OrganismCodingDataModel
    {
        /// <summary>
        /// Gets or sets the code allocated to the organism.
        /// </summary>
        public required string Code { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the organism table to the organismcoding table.
        /// </summary>
        public int OrganismId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table (which contains the coding list id) to the organismcoding table.
        /// </summary>
        public int CodingId { get; set; }
    }
}
