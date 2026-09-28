
using System.Collections.Generic;

namespace arc.domain.Coding
{
    public class Breakpoint : Organism
    {
        public int AntibioticId { get; set; }
        public string SpecimenTypeId { get; set; }
        public int TestMethodId { get; set; }
        public int SpecificationId { get; set; }
        public int SpecialConsiderId { get; set; }
        public int HostId { get; set; }
        public string Dosage { get; set; }
        public string Enabled { get; set; }
        public int MetafCodingId { get; set; }
        public List<BreakpointLine> BreakpointGrid { get; set; }
        public string Order { get; set; }
        public string Family { get; set; }
        public string OrganismGroup { get; set; }
    }
}
