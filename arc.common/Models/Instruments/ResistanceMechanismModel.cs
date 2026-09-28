using Newtonsoft.Json;

namespace arc.common.Models.Instruments;

/// <summary>
/// Expert resistance mechanism from an instrument (e.g. Vitek2 <c>ExpertFinding</c> / MIS payload).
/// </summary>
public class ResistanceMechanismModel
{
    /// <summary>
    /// Drug family reported by the instrument (e.g. Vitek <c>drugFamily</c>).
    /// </summary>
    [JsonProperty("drugFamily")]
    public string DrugFamily { get; set; } = "";

    /// <summary>
    /// Phenotype reported by the instrument (e.g. Vitek <c>PhenoType</c>).
    /// </summary>
    [JsonProperty("phenoType")]
    public string PhenoType { get; set; } = "";
}
