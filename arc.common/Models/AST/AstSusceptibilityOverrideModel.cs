using System;

namespace arc.common.Models.AST;

/// <summary>
/// Portal and API DTO for manual susceptibility override audit on an AST line.
/// </summary>
public class AstSusceptibilityOverrideModel
{
    /// <summary>When true, susceptibility must not be overwritten by breakpoint lookup.</summary>
    public bool IsManuallySet { get; set; }

    /// <summary>Username that set the manual susceptibility.</summary>
    public string SetByUsername { get; set; } = "";

    /// <summary>When the manual susceptibility was set.</summary>
    public DateTime? SetAt { get; set; }

    /// <summary>Canned reason id from list 145 ASTSusOverrideCanned (0 = none).</summary>
    public int CannedCommentId { get; set; }

    /// <summary>Free-text override reason.</summary>
    public string FreeTextComment { get; set; } = "";

    /// <summary>Prior susceptibility list-item id before override (0 if none).</summary>
    public int OverriddenFromSusceptibilityId { get; set; }

    /// <summary>When true, save should delete the override row for this line (revert to calculated).</summary>
    public bool ClearOverride { get; set; }
}
