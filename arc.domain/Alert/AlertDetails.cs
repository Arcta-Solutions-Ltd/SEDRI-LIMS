using arc.domain.Coding;
using System.Collections.Generic;

namespace arc.domain.Alert
{
    public class AlertDetails : Organism
    {
        public string AlertName { get; set; }
        public int SeroTypeId { get; set; }
        public string OrganismConfigured
        {
            get
            {
                return IsOrganismConfigured() ? "Yes" : "No";
            }
        }
        public int AlertTypeId { get; set; }
        /// <summary>
        /// Gets or sets the specification ID (FK to specification).
        /// </summary>
        public int SpecificationId { get; set; }
        public string AlertMessage { get; set; }
        public string SusceptibilityAndOr { get; set; }
        public string TestAndOr { get; set; }
        public string Enabled { get; set; }
        public string DoesExist { get; set; }
        public string TagId { get; set; }
        public string Order { get; set; }
        public string Family { get; set; }
        public string OrganismGroup { get; set; }
        public List<TestGrid> TestGrid { get; set; } = [];
        public List<SusceptibilityGrid> SusceptibilityGrid { get; set; } = [];

        private bool IsOrganismConfigured()
        {
            return (OrderId != 0 || FamilyId != 0 || GenusId != 0 || SpeciesId != 0 || SubSpeciesId != 0 || SeroTypeId != 0 || OrgGroupCodingId != 0);
        }
    }
}

