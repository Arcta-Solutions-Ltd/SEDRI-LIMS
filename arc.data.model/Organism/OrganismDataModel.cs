namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the organism table in the database.
    /// </summary>
    public class OrganismDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the genus table to the organism table.
        /// </summary>
        public int GenusId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the species table to the organism table.
        /// </summary>
        public int SpeciesId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the subspecies table to the organism table.
        /// </summary>
        public int SubSpeciesId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the additional table to the organism table.
        /// </summary>
        public int AdditionalId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the serotype table to the organism table.
        /// </summary>
        public int SerotypeId { get; set; }
    }
}
