namespace arc.common.Models.Coding;
public class RuleConditionGridModel
{
    public int? ConditionId { get; set; }
    public int ExpertRuleId { get; set; }
    public int TestMethodId { get; set; }
    public int? AntibioticId { get; set; }
    public int? AntibioticGroupId { get; set; }
    public int SusceptibilityId { get; set; }
    public int SpecialConsiderationId { get; set; }
    public decimal? StartVal { get; set; }
    public decimal? EndVal { get; set; }
}
