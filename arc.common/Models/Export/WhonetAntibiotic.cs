namespace arc.common.Models.Export
{
    public class WhonetAntibiotic
    {
        public int CultureId { get; set; }
        public string Antibiotic { get; set; }
        public int AntibioticId { get; set; }
        public string AntibioticCode { get; set; }
        public string Susceptibility { get; set; }
        public int TestMethodId { get; set; }
        public int Dosage { get; set; }
        public int GuidelinesId { get; set; }
        public string Measurement { get; set; }
        public string MICComparison { get; set; }
    }
}
