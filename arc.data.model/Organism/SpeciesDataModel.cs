namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the species table in the database.
    /// </summary>
    public class SpeciesDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the species.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the genus table to the species table.
        /// </summary>
        public int GenusId { get; set; }
    }
}
