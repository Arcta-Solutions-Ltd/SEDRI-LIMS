using arc.common.Models.Alert;
using System;
using System.Collections.Generic;

namespace arc.domain.Alert
{
    public class AlertModel
    {
        public int Id { get; set; }
        public string AlertName { get; set; }
        public int OrganismId { get; set; }
        public string AlertMessage { get; set; }
        public string SusceptibilityAndOr { get; set; }
        public string TestAndOr { get; set; }
        public string Enabled { get; set; }
        public string DoesExist { get; set; }
        public List<TestGrid> TestGrid { get; set; }
        public List<SusceptibilityGrid> SusceptibilityGrid { get; set; }

        public static explicit operator AlertModel(AlertDetailsModel v)
        {
            throw new NotImplementedException();
        }
    }
}
