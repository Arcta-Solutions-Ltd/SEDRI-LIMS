namespace arc.common.Models.AST;
public class ASTOutboundModel
{
    public int DrugCategory { get; set; }
    public int? AntibioticId { get; set; }
    public int OrganismId { get; set; }
    public int Dosage { get; set; }
    public int Guidelines { get; set; }
    public int? ZoneDiameter { get; set; }
    public int TestResult { get; set; }
    public decimal? Mic { get; set; }
    public string IncludeOnReport { get; set; }
    public int SpecialConsiderationId { get; set; }
    public bool ExpertRuleLine { get; set; }
    public string ExpertRuleName { get; set; }
    public string ExpertRuleText { get; set; }
}
