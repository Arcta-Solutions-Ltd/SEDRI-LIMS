namespace arc.data.model.Quality
{
    /// <summary>
    /// Represents the fields in the iqctestprofileqcorganisms table in the database.
    /// </summary>
    public class IqcTestProfileQcorganismsDataModel
    {
        public int IqcTestProfileId { get; set; }
        public int QcOrganismId { get; set; }
        public bool UseByDefault { get; set; }
    }
}
