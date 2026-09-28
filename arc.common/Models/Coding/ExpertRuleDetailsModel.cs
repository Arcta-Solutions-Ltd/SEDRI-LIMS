using System.Collections.Generic;

namespace arc.common.Models.Coding;
public class ExpertRuleDetailsModel
{
    public int Id { get; set; }
    public string ExpertRuleName { get; set; }
    public string RuleText { get; set; }
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
    public int SpecificationId { get; set; }
    public int RuleCategoryId { get; set; }
    public string CombinationRule { get; set; }
    public string Enabled { get; set; }
    public string AlertOnRule { get; set; }
    public string TagId { get; set; }
    public string Order { get; set; }
    public string Family { get; set; }
    public string OrganismGroup { get; set; }
    public string SpecimenTypesToInclude { get; set; }
    public string SpecimenTypesToExclude { get; set; }
    public List<RuleConditionGridModel> RuleConditionGrid { get; set; } = [];
    public List<RuleTestConditionGridModel> RuleTestConditionGrid { get; set; } = [];
    public List<RuleActionGridModel> RuleActionGrid { get; set; } = [];
}
