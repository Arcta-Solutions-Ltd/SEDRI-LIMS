namespace arc.domain.Coding
{
    public class TestPatternLine
    {
        public int Id { get; set; }
        public int testOrder { get; set; }
        public int? AntibioticId { get; set; }
        public string Dosage { get; set; }
        public int TestMethodId { get; set; }
        public int GuidelinesId { get; set; }
        public int CategoryId { get; set; }
        public bool PrintOnReport { get; set; }
    }
}
