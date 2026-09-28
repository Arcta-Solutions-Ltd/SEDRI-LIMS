using arc.common.Models.AST;

namespace arc.common.Models.Coding
{
    public class TestPatternScopeModel : TestPatternSummaryModel
    {
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
        public int SubSpeciesId { get; set; }
        public int SerotypeId { get; set; }
        public int AdditionalId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public int ScopeOrganismId { get; set; }
        public bool IsOrganismGroupMatch { get; set; }
        public int HostId { get; set; }
        public string MakeDefault { get; set; }
        public string SpecimenTypes { get; set; }
    }
}
