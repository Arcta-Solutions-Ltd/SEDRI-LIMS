namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the organismsynonym table in the database.
    /// </summary>
    public class OrganismSynonymDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the organism table to the organismsynonym table.
        /// </summary>
        public int OrganismId { get; set; }
        /// <summary>
        /// Gets or sets boolean indicating whether this synonym is the preferred name for the organism.
        /// </summary>
        public bool PreferredName { get; set; }
        /// <summary>
        /// Gets or sets the synonym name for the organism.
        /// </summary>
        public required string Synonym { get; set; }
    }
}
