using System.Collections.Generic;

namespace arc.domain.Alert
{
    public class CultureAlert
    {
        public int Id { get; set; }
        public int SpecimenAlertId { get; set; }
	    public int CultureId { get; set; }
        public List<CultureTestAlert> CultureTestAlerts { get; set; }
        public List<ASTTestAlert> ASTTestAlerts { get; set; }
    }
}
