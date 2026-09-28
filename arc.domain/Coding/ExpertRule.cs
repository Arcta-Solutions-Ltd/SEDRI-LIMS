using arc.common.Models.Coding;
using System.Collections.Generic;

namespace arc.domain.Coding
{
    public class ExpertRule : Organism
    {
        public int RuleId {  get; set; }
        public string ExpertRuleName { get; set; }
        public string RuleText { get; set; }
        public int RuleCategoryId { get; set; }
        public string OrganismConfigured
        {
            get
            {
                return IsOrganismConfigured() ? "Yes" : "No";
            }
        }
        public int SpecificationId { get; set; }
        /// <summary>
        /// Guidelines list item id from the linked specification (specification.guidelinesid), for AST display.
        /// </summary>
        public int SpecificationGuidelinesId { get; set; }
        public int TestMethodId { get; set; }
        public string CombinationRule { get; set; }
        public string Enabled { get; set; }
        public string AlertOnRule { get; set; }
        public string Order { get; set; }
        public string Family { get; set; }
        public string OrganismGroup { get; set; }
        public int SeroTypeId { get; set; }
        public string SpecimenTypesToInclude { get; set; }
        public string SpecimenTypesToExclude { get; set; }
        public string TagId { get; set; }
        public List<RuleActionGridModel> RuleActionGrid { get; set; } = [];
        public List<RuleTestConditionGridModel> RuleTestConditionGrid { get; set; } = [];
        public List<RuleConditionGridModel> RuleConditionGrid { get; set; } = [];
        private bool IsOrganismConfigured()
        {
            return (OrderId != 0 || FamilyId != 0 || GenusId != 0 || SpeciesId != 0 || SubSpeciesId != 0 || SeroTypeId != 0 || OrgGroupCodingId != 0);
        }
    }
}