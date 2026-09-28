using System;

namespace arc.common.Models.Coding
{
    public class OrganismDescriptorModel
    {
        public int OrganismId { get; set; }
        public int FamilyId { get; set; }
        public int OrderId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public int AdditionalId { get; set; }
        public int SubspeciesId { get; set; }
        public int SerotypeId { get; set; }
    }
}
