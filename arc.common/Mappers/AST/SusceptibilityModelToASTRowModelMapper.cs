using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Mappers.AST;

/// <summary>
/// Maps susceptibility (breakpoint) models to AST row models for display on the AST screen.
/// Used for both standard breakpoints and special consideration breakpoints.
/// </summary>
public class SusceptibilityModelToASTRowModelMapper : IMapWithList<SusceptibilityModel, ASTRowModel>
{
    /// <summary>
    /// Maps a susceptibility model to an AST row model.
    /// </summary>
    public ASTRowModel Map(SusceptibilityModel source)
    {
        var result = source.Map<ASTRowModel>();
        result.Antibiotic = source.AntibioticId;
        result.Guidelines = source.GuidelinesId;
        result.TestMethod = source.TestMethodId;
        result.TestResult = source.SusceptibilityId;
        result.SpecialConsideration = source.SpecialConsideration;
        result.SpecialConsiderationId = source.SpecialConsiderationId;
        result.DrugCategory = source.CategoryId;
        result.AppliedBreakpointId = source.BreakpointId;
        return result;
    }

    public List<ASTRowModel> MapToList(SusceptibilityModel source)
    {
        var result = new List<ASTRowModel> { Map(source) };
        return result.ToList();

    }

    public List<ASTRowModel> MapList(List<SusceptibilityModel> source)
    {
        var result = source.Select(s => Map(s));
        return result.ToList();
    }
}

