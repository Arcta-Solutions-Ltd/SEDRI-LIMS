using System.Collections.Generic;

namespace arc.domain.Alert
{
    public class SpecimenAlertCommandModel
    {
        public List<SpecimenAlert> Alerts { get; set; }
        public int SpecimenId { get; set; }
    }
}
