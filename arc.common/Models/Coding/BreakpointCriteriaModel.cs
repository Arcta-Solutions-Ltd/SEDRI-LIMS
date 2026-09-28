namespace arc.common.Models.Coding
{
    public class BreakpointCriteriaModel
    {
        public int OrganismId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public int AntibioticId { get; set; }
        public int SpecimenTypeId { get; set; }
        public int TestMethodId { get; set; }
        public int SourceId { get; set; }
        public int SpecialConsiderId { get; set; }
        public int HostId { get; set; }
        public string Dosage { get; set; }
    }
}