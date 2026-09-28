namespace arc.common.Models.Alert;

/// <summary>
/// View model for displaying alert details on the alert record view.
/// All list-item foreign keys are resolved to display text.
/// </summary>
public class AlertViewModel
{
    public int Id { get; set; }
    public string AlertName { get; set; }
    public string AlertMessage { get; set; }
    public string OrganismName { get; set; }
    public string OrderName { get; set; }
    public string FamilyName { get; set; }
    public string OrgGroupName { get; set; }
    public string AlertType { get; set; }
    /// <summary>
    /// Gets or sets the specification display text (e.g. "EUCAST - Breakpoint Tables (2020, 2024)").
    /// </summary>
    public string Specification { get; set; }
    public string Enabled { get; set; }
    public string SusceptibilityAndOr { get; set; }
    public string TestAndOr { get; set; }
}
