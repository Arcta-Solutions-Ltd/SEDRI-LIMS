using System.Collections.Generic;

namespace arc.common.Models.AST;

/// <summary>
/// Request body for refreshing expert rule groups from the current AST disk/MIC state (portal edit flow).
/// </summary>
public class ExpertRulesForPageRequest
{
    /// <summary>Culture (isolate) id for the AST form.</summary>
    public int CultureId { get; set; }

    public List<ASTRowModel> DiskResults { get; set; } = [];

    public List<ASTRowModel> MicResults { get; set; } = [];
}
