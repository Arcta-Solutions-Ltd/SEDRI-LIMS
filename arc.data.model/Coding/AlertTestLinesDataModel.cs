namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the alerttestlines table in the database.
    /// </summary>
    public class AlertTestLinesDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the alert table to the alerttestlines table.
        /// </summary>
        public int AlertId { get; set; }
        /// <summary>
        /// Gets or sets the name of the test to use in an alert condition.
        /// </summary>
        public string? TestName { get; set; }
        /// <summary>
        /// Gets or sets the name of the field to use in an alert condition.
        /// </summary>
        public string? FieldName { get; set; }
        /// <summary>
        /// Gets or sets the name type of comparison to use in an alert condition.
        /// </summary>
        public string? Comparison { get; set; }
        /// <summary>
        /// Gets or sets the comparison value to use in an alert condition.
        /// </summary>
        public string? CompValue { get; set; }
    }
}
