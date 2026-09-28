using System.Collections.Generic;
using arc.common.Utils;
using Newtonsoft.Json;

namespace arc.common.Models.AST;

/// <summary>
/// Portal JSON for <c>POST AST/getexpertrulesforpage</c> may send <c>null</c> for unset numeric fields;
/// non-nullable <see cref="int"/> properties use <see cref="JsonInt32NullAsZeroConverter"/>.
/// </summary>
public class ASTRowModel
{
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int DrugCategory { get; set; }
    public int? Antibiotic { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int Dosage { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int Guidelines { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int TestMethod { get; set; }
    public int? ZoneDiameter { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int TestResult { get; set; }
    public decimal? Mic { get; set; }
    public string IncludeOnReport { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int SpecialConsiderationId { get; set; }
    public string SpecialConsideration { get; set; }
    public bool ExpertRuleLine { get; set; }
    public string ExpertRuleName { get; set; }
    public string ExpertRuleText { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int ExpertRuleId { get; set; }
    /// <summary>
    /// Expert rule action targets antibiotic display on report only (no susceptibility). Not used for manual AST rows.
    /// </summary>
    public bool IsPrintOnReportOnlyExpertAction { get; set; }
    public bool PotentialRule { get; set; }
    public List<ExpertRuleConditionReturnModel> RuleConditions { get; set; }
    public List<ASTRowModel> EmbeddedASTRows { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int OrganismId { get; set; }
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int SpecimenTypeId { get; set; }
    /// <summary>
    /// Culture's laboratory; used to filter expert rules to antibiotics in the lab's selected antibiotic groups.
    /// </summary>
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int LaboratoryId { get; set; }
    public string Operator { get; set; }
    /// <summary>
    /// <c>breakpoint.id</c> that produced this row's susceptibility (0 when manual or expert-rule line).
    /// Traceability to specification via <c>breakpoint.specificationid</c>.
    /// </summary>
    [JsonConverter(typeof(JsonInt32NullAsZeroConverter))]
    public int AppliedBreakpointId { get; set; }

    /// <summary>
    /// Manual susceptibility override audit (loaded from astsusceptibilityoverride; not stored on ast table).
    /// </summary>
    public AstSusceptibilityOverrideModel SusceptibilityOverride { get; set; }

    /// <summary>
    /// When true, server runs full breakpoint lookup even if override would normally protect TestResult.
    /// </summary>
    public bool ForceRecalculateSusceptibility { get; set; }
}
