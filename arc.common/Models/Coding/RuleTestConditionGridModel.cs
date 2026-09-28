namespace arc.common.Models.Coding;
public class RuleTestConditionGridModel
{
    public int? TestConditionId { get; set; }
    public int ExpertRuleId { get; set; }
    public string TestName { get; set; }
    public string FieldName { get; set; }
    public string Comparison { get; set; }
    public string CompValue { get; set; }

    // Properties for crafted TestGrid format from frontend
    // These are populated when data comes from the crafted TestGrid and transformed in the mapper
    public string Test { get; set; }
    public string Field { get; set; }
    public string StringValue { get; set; }
    public string NumberValue { get; set; }
    public string ListValue { get; set; }
}
