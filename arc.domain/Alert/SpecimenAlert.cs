using System.Collections.Generic;

namespace arc.domain.Alert
{
    public class SpecimenAlert
    {
        public int Id { get; set; }
        public int SpecimenId { get; set; }
        public int CultureId { get; set; }
        public int AlertId { get; set; }
        public int AlertTypeId { get; set; }
        public string TagId { get; set; }
        public List<CultureAlert> CultureAlerts { get; set; }
        public List<SpecimenTestAlert> SpecimenTestAlerts { get; set; }
    }
}
