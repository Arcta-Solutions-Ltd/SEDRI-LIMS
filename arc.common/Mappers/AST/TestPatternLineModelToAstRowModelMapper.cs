using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using arc.common.Models.Coding;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Mappers.AST;
public class TestPatternLineModelToASTRowModelMapper : IMapWithList<TestPatternLineModel, ASTRowModel>
{
    /// <summary>
    /// Maps a test pattern line to an AST row model. Test pattern rows always have ExpertRuleLine false;
    /// expert rule rows in the AST come from applied rules (ExpertRuleAction), not from the test pattern.
    /// MIC lines may have null or empty Dosage; in that case Dosage is set to 0.
    /// </summary>
    /// <param name="source">The test pattern line to map.</param>
    /// <returns>The mapped AST row model.</returns>
    public ASTRowModel Map(TestPatternLineModel source)
    {
        var result = source.Map<ASTRowModel>();
        result.Antibiotic = source.AntibioticId;
        result.Guidelines = source.GuidelinesId;
        result.TestMethod = source.TestMethodId;
        result.Dosage = int.TryParse(source.Dosage, out var dosage) ? dosage : 0;
        result.DrugCategory = source.CategoryId;
        result.TestResult = 0;
        result.ExpertRuleLine = false;
        result.SpecialConsiderationId = 973;
        result.IncludeOnReport = source.PrintOnReport == "True" ? "Yes" : "No";
        return result;
    }

    public List<ASTRowModel> MapToList(TestPatternLineModel source)
    {
        var result = new List<ASTRowModel> { Map(source) };
        return result.ToList();
    }

    public List<ASTRowModel> MapList(List<TestPatternLineModel> source)
    {
        var result = source.Select(s => Map(s));

        return result.ToList();
    }
}

