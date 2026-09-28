using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace arc.common.Mappers.AST;

/// <summary>
/// Maps expert rule action data to AST row model for display on susceptibility reports.
/// </summary>
public class ExpertRuleActionReturnModelToASTRowModelMapper : IMapWithList<ExpertRuleActionReturnModel, ASTRowModel>
{
    /// <summary>
    /// Maps an expert rule action to an AST row. IncludeOnReport is derived from DisplayOnReport;
    /// an unset (null or whitespace) DisplayOnReport maps to a null IncludeOnReport so the AST screen
    /// renders an editable toggle (defaulting to Yes) instead of read-only Yes/No text.
    /// </summary>
    /// <param name="source">The expert rule action to map.</param>
    /// <returns>An AST row model populated with the action data.</returns>
    public ASTRowModel Map(ExpertRuleActionReturnModel source)
    {
        var result = source.Map<ASTRowModel>();
        result.Antibiotic = source.AntibioticId;
        result.Guidelines = source.SourceId;
        result.TestResult = source.SusceptibilityId;
        result.ExpertRuleLine = true;
        result.ExpertRuleName = source.ExpertRuleName;
        result.ExpertRuleText = source.RuleText;
        result.ExpertRuleId = source.ExpertRuleId;
        result.IncludeOnReport = string.IsNullOrWhiteSpace(source.DisplayOnReport) ? null : source.DisplayOnReport;
        return result;
    }

    /// <summary>
    /// Maps an expert rule action to a single-element list of AST row models.
    /// </summary>
    /// <param name="source">The expert rule action to map.</param>
    /// <returns>A list containing one AST row model.</returns>
    public List<ASTRowModel> MapToList(ExpertRuleActionReturnModel source)
    {
        var result = new List<ASTRowModel> { Map(source) };
        return result.ToList();

    }

    /// <summary>
    /// Maps a list of expert rule actions to AST row models.
    /// </summary>
    /// <param name="source">The list of expert rule actions to map.</param>
    /// <returns>A list of AST row models.</returns>
    public List<ASTRowModel> MapList(List<ExpertRuleActionReturnModel> source)
    {
        var result = source.Select(s => Map(s));
        return result.ToList();
    }
}

