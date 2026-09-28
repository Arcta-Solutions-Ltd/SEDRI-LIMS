namespace arc.common.Models.Laboratory;

/// <summary>
/// Laboratory antibiotic group selection and resolved antibiotic group table IDs for AST expert-rule filtering.
/// </summary>
public class LaboratoryAntibioticGroupFilterInfo
{
    public string AntibioticGroupIdsRaw { get; set; }
    public int[] AllowedAntibioticGroupIds { get; set; }
}
