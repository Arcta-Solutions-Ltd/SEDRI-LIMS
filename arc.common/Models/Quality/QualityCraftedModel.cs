using arc.common.Models.Role;
using System.Collections.Generic;

namespace arc.common.Models.QualityAssurance
{
    public class QualityCraftedModel
    {
        public List<ContentsCraftedModel> Crafted { get; set; }
        public string Default { get; set; }
        public string Enabled { get; set; }
        public int Id { get; set; }
        public int MetaFListItemId { get; set; }
    }

    public class ContentsCraftedModel
    {
        public string Name { get; set; }
        public List<CraftedSelectionsModel> Contents { get; set; }
    }
}
