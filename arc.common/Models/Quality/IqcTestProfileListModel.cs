namespace arc.common.Models.QualityAssurance
{
    public class IqcTestProfileListModel
    {
        public int Id { get; set; }
        public string Organism { get; set; }
        public string StandardsBody { get; set; }
        public string UseByDefault { get; set; }
        public string Enabled { get; set; }
        public string PrimaryStrain { get; set; }
        public string OtherStrains { get; set; }
    }
}
