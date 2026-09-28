using arc.common.Models.AST;

using System.Collections.Generic;

using ExpertRuleModel = arc.domain.Coding.ExpertRule;



namespace arc.app.ExpertRule;



/// <summary>

/// Inputs required to evaluate which organism-scoped expert rules are valid for display given current AST and isolate-test data.

/// </summary>

public class ExpertRuleDisplayValidityContext

{

    /// <summary>Rules returned by <c>RetrieveExpertRulesAsync</c> (including grids).</summary>

    public IReadOnlyList<ExpertRuleModel> Rules { get; set; }



    /// <summary>

    /// MIC/disk AST rows used for rule conditions, typically flattened from disk/MIC results (including embedded rows).

    /// </summary>

    public IReadOnlyList<ASTRowModel> AstRows { get; set; }



    /// <summary>

    /// Parsed key/value lines from culture tests, scoped per test name for <see cref="RuleTestConditionGridModel"/> matching.

    /// </summary>

    public IReadOnlyList<CultureTestFieldLine> CultureTestFields { get; set; }



    /// <summary>Current culture's specimen type id for include/exclude lists on the rule.</summary>

    public int SpecimenTypeId { get; set; }



    /// <summary>

    /// Maps <c>antibiotic.id</c> to the set of antibiotic group listitem ids (<c>antibioticcoding.codingid</c>)

    /// the antibiotic belongs to.

    /// </summary>

    public IReadOnlyDictionary<int, HashSet<int>> AntibioticIdToCodingIds { get; set; }



    /// <summary>

    /// When true (default), rules with <c>Enabled != "Yes"</c> are excluded.

    /// </summary>

    public bool OnlyEnabledRules { get; set; } = true;



    /// <summary>

    /// List item id stored in <see cref="ExpertRuleModel.CombinationRule"/> meaning AND between AST and test condition groups (same convention as alerts; default <c>987</c>).

    /// </summary>

    public string CombinationRuleAndListItemId { get; set; } = "987";

}

