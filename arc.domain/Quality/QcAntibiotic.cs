using System;

namespace arc.domain.Quality
{
    public class QcAntibiotic
    {
        public int Id { get; set; }
        public int QcOrganismId { get; set; }
        public int AntibioticId { get; set; }
        public bool Enabled { get; set; }
        public float MicTargetLower { get; set; }
        public float MicTargerUpper { get; set; }
        public float MicRangeLower { get; set; }
        public float MicRangeUpper { get; set; }
        public string DiskContent { get; set; }
        public float InhibitionZoneDiameterTargetLower { get; set; }
        public float InhibitionZoneDiameterTargetUpper { get; set; }
        public float InhibitionZoneDiameterRangeLower { get; set; }
        public float InhibitionZoneDiameterRangeUpper { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
