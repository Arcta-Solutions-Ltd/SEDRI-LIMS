namespace arc.common.Models.Alert;

/// <summary>
/// Model for displaying test criteria in the alert record view list section.
/// </summary>
public class AlertTestCriteriaModel
{
    public int Id { get; set; }
    public string TestName { get; set; }
    public string FieldName { get; set; }
    public string Comparison { get; set; }
    public string CompValue { get; set; }
}
