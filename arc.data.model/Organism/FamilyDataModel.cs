namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the family table in the database.
    /// </summary>
    public class FamilyDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the order table to the family table.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the family.
        /// </summary>
        public required string Name { get; set; }
    }
}
