namespace arc.data.model.AST;

/// <summary>
/// Persisted manual susceptibility override audit for one AST line (parent or special-consideration embed).
/// </summary>
public class AstSusceptibilityOverrideDataModel : IdAndDateBase
{
    public int CultureId { get; set; }
    public int TestMethodId { get; set; }
    public int AntibioticId { get; set; }
    public int GuidelinesId { get; set; }
    public int Dosage { get; set; }
    public int SpecialConsiderationId { get; set; }
    public string IsManuallySet { get; set; } = "Yes";
    public string SetBy { get; set; } = "";
    public DateTime? SetAt { get; set; }
    public int CannedCommentId { get; set; }
    public string FreeTextComment { get; set; } = "";
    public int OverriddenFromSusceptibilityId { get; set; }
}
