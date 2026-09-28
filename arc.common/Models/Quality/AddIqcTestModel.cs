using System.Collections.Generic;

namespace arc.common.Models.Quality
{
    public class AddIqcTestModel
    {
        public int TestProfileId { get; set; }
        public List<int> IqcTestProfileQcOrganismIds { get; set; } = new List<int>();
    }
}
