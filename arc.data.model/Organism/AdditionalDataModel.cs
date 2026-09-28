namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the additional table in the database.
    /// </summary>
    public class AdditionalDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the organism.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the family table to the additional table.
        /// </summary>
        public int FamilyId { get; set; }
    }
}
