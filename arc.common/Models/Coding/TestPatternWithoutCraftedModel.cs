using System.Collections.Generic;

namespace arc.common.Models.Coding
{
    public class TestPatternWithoutCraftedModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
        public int OrganismId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public string TestPatternName { get; set; }
        public int MetafCodingId { get; set; }
        public int HostId { get; set; }
        public string MakeDefault { get; set; }
        public List<TestPatternLineModel> AntibioticGrid { get; set; }
        public string SpecimenTypeId { get; set; }
    }
}
