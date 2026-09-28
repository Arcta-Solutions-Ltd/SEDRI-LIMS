namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the subspecies table in the database.
    /// </summary>
    public class SubspeciesDataModel
    {
        /// <summary>
        /// Gets or sets the name of the subspecies.
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the species table to the subspecies table.
        /// </summary>
        public int SpeciesId { get; set; }
    }
}
