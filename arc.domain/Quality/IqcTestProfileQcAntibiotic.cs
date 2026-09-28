using System;

namespace arc.domain.Quality
{
    public class IqcTestProfileQcAntibiotic
    {
        public int Id { get; set; }
        public int IqcTestProfileQcOrganismId { get; set; }
        public int QcAntibioticId { get; set; }
        public bool Enabled { get; set; }
        public DateTime LastModifiedDate { get; set; } = DateTime.Now;

        public IqcTestProfileQcAntibiotic() { }

        public IqcTestProfileQcAntibiotic(int iqcTestProfileQcOrganismId, int qcAntibioticId, bool enabled)
        {
            IqcTestProfileQcOrganismId = iqcTestProfileQcOrganismId;
            QcAntibioticId = qcAntibioticId;
            Enabled = enabled;
        }
    }
}
