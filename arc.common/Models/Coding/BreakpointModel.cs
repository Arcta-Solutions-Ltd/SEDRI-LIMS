using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class BreakpointModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public int AntibioticId { get; set; }
        public string SpecimenTypeId { get; set; }
        public int TestMethodId { get; set; }
        public int SpecificationId { get; set; }
        public int SpecialConsiderId { get; set; }
        public int HostId { get; set; }
        public string Dosage { get; set; }
        public string Enabled { get; set; }
        public int MetafCodingId { get; set; }
        public List<BreakpointLineModel> BreakpointGrid { get; set; }
        public List<CraftedModel> Crafted { get; set; }
        public int OrganismId { get; set; }

    }
}
