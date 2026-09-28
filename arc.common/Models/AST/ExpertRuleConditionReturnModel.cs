namespace arc.common.Models.AST;
public class ExpertRuleConditionReturnModel
{
    public int Id { get; set; }
    public int ExpertRuleId { get; set; }
    public int AntibioticId { get; set; }
    public int TestMethodId { get; set; }
    public int SusceptibilityId { get; set; }
    public decimal StartVal { get; set; }
    public decimal EndVal { get; set; }
}
