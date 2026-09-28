using System.Collections.Generic;

namespace arc.domain.Coding
{
    public class TestPattern : Organism
    {
        public string TestPatternName { get; set; }
        public string SpecimenTypeId { get; set; }
        public int MetafCodingId { get; set; }
        public int HostId { get; set; }
        public string MakeDefault { get; set; }
        public List<TestPatternLine> AntibioticGrid { get; set; }
        public string Order { get; set; }
        public string Family { get; set; }
        public string OrganismGroup { get; set; }
    }
}
