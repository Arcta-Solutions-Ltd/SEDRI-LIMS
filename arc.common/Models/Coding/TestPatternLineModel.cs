using System;

namespace arc.common.Models.Coding
{
    public class TestPatternLineModel
    {
        public int Id { get; set; }
        public int testOrder { get; set; }
        public int? AntibioticId { get; set; }
        public string Dosage { get; set; }
        public int TestMethodId { get; set; }
        public int GuidelinesId { get; set; }
        public int CategoryId { get; set; }
        public string PrintOnReport { get; set; }
    }
}