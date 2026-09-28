namespace arc.data.model.Organism
{
    /// <summary>
    /// Represents the fields in the ordercat table in the database.
    /// </summary>
    public class OrderCatDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the order.
        /// </summary>
        public required string Name { get; set; }
    }
}
