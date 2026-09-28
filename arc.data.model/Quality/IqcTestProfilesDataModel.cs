namespace arc.data.model.Quality
{
    /// <summary>
    /// Represents the fields in the iqctestprofiles table in the database.
    /// </summary>
    public class IqcTestProfilesDataModel : IdAndDateBase
    {
        public string? Name { get; set; }
        public int TestMethodListItemId { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
