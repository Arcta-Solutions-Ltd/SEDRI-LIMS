namespace arc.common.Models.QualityAssurance
{
    public class IqcResultGridViewModel
    {
        public int Id { get; set; }
        public string OrganismName { get; set; }
        public string AntibioticName { get; set; }
        public decimal? Value { get; set; }
        public string TestMethod { get; set; }
        public decimal? RangeLower { get; set; }
        public decimal? RangeUpper { get; set; }
        public bool WithinRange { get; set; }
        public int AlertCategoryId { get; set; }
        public string AlertTitle { get; set; }
        public string Colour { get; set; }
        public string PrimaryStrain { get; set; }
        public string StandardsBody { get; set; }
    }
}
