using System.Collections.Generic;

namespace arc.common.Models.AST;

/// <summary>
/// Serialized snapshot of manual Disk/MIC rows that were omitted from <see cref="ASTUpdateEventModel.ASTResults"/>
/// because an applied expert rule suppressed them. Persisted in <c>cultureastexpertruleevalcontext</c>, not in the <c>AST</c> table.
/// </summary>
public class ExpertRuleEvalContextModel
{
    /// <summary>Suppressed manual disk rows (typically <c>TestMethod</c> 681).</summary>
    public List<ASTRowModel>? DiskResults { get; set; }

    /// <summary>Suppressed manual MIC rows (typically <c>TestMethod</c> 680).</summary>
    public List<ASTRowModel>? MicResults { get; set; }
}
