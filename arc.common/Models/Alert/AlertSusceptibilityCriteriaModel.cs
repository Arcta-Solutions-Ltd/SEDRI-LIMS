namespace arc.common.Models.Alert;

/// <summary>
/// Model for displaying susceptibility criteria in the alert record view list section.
/// Antibiotic names and susceptibility are resolved to text.
/// </summary>
public class AlertSusceptibilityCriteriaModel
{
    public int Id { get; set; }
    public string AntibioticNames { get; set; }
    public string SusceptibilityName { get; set; }
}
