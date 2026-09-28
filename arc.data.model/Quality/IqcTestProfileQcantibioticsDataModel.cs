namespace arc.data.model.Quality
{
    /// <summary>
    /// Represents the fields in the iqctestprofileqcantibiotics table in the database.
    /// </summary>
    public class IqcTestProfileQcantibioticsDataModel : IdAndDateBase
    {
        public int IqcTestProfileQcorganismId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the antibiotic table to the iqctestprofileqcantibiotics table.
        /// </summary>
        public int QcantibioticId { get; set; }
        /// <summary>
        /// Gets or sets the value of the Enabled flag for the antibiotic quality rule. 
        /// </summary>
        public bool Enabled { get; set; }
    }
}

