namespace arc.common.Models.Quality
{
    public class IqcTestProfileQcOrganismsModel
    {
        public int Id { get; set; }
        public string Organism { get; set; }
        public string StandardsBody { get; set; }
        public bool PresentInTest { get; set; }
        public string PrimaryStrain { get; set; }
        public string OtherStrains { get; set; }
    }
}
