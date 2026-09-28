namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the genus table in the database.
    /// </summary>
    public class GenusDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the family table to the genus table.
        /// </summary>
        public int FamilyId { get; set; }
        /// <summary>
        /// Gets or sets the name of the genus.
        /// </summary>
        public required string Name { get; set; }
    }
}
