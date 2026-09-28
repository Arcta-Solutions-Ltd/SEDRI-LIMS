using System.Collections.Generic;

namespace arc.common.Models.Alert
{
    public class AlertDetailsModel
    {
        public int Id { get; set; }
        public string AlertName { get; set; }
        public int OrderId { get; set; }
        public int FamilyId { get; set; }
        public int GenusId { get; set; }
        public int SpeciesId { get; set; }
        public int SubSpeciesId { get; set; }
        public int SerotypeId { get; set; }
        public int AdditionalId { get; set; }
        public int SeroTypeId { get; set; }
        public int OrganismId { get; set; }
        public int OrgGroupCodingId { get; set; }
        public string OrganismConfigured { get; set; }
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
        public string MoreData { get; set; }
        public List<TestGridModel> TestGrid { get; set; } = [];
        public List<SusceptibilityGridModel> SusceptibilityGrid { get; set; } = [];
    }
}
