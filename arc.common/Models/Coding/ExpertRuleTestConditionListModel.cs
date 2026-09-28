namespace arc.common.Models.Coding;

/// <summary>
/// List model for displaying expert rule test conditions in the record view embedded list.
/// Database stores stable ids; list display properties hold resolved titles and field labels after enrichment.
/// </summary>
public class ExpertRuleTestConditionListModel
{
    public int Id { get; set; }
    public string TestName { get; set; }
    public string FieldName { get; set; }
    public string Comparison { get; set; }
    public string CompValue { get; set; }
}
