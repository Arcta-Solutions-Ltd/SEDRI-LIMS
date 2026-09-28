using System;
using System.Collections.Generic;

namespace arc.domain.Quality
{
    public class IqcTestProfileQcOrganism
    {
        public int Id { get; set; }
        public int IqcTestProfileId { get; set; }
        public int QcOrganismId { get; set; }
        public bool UseByDefault { get; set; } = false;
        public List<IqcTestProfileQcAntibiotic> IqcTestProfileQcAntibiotics { get; set; } = new List<IqcTestProfileQcAntibiotic>();
        public DateTime LastModifiedDate { get; set; }
        public IqcTestProfileQcOrganism() { }
        public IqcTestProfileQcOrganism(int qcOrganismId, int listItemId, bool useByDefault)
        {
            QcOrganismId = qcOrganismId;
            _ = listItemId;
            UseByDefault = useByDefault;
        }
    }
}
