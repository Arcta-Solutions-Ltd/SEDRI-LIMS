using arc.common.Models.AST;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.AST;

/// <summary>
/// Enriches AST rows with special consideration rows derived from breakpoints.
/// Used when loading AST data (existing results or test pattern) to ensure all special
/// consideration rows for each antibiotic/guideline/dosage are displayed.
/// </summary>
public interface ISpecialConsiderationEnricher
{
    /// <summary>
    /// Enriches with special consideration rows for the given antibiotic/guideline/dosage/organism.
    /// Merges breakpoint-derived rows with stored embedded rows; stored values take precedence.
    /// </summary>
    /// <param name="organismId">Organism ID for breakpoint lookup.</param>
    /// <param name="antibioticId">Antibiotic ID.</param>
    /// <param name="guidelinesId">Guidelines/source ID.</param>
    /// <param name="dosage">Dosage (0 for MIC).</param>
    /// <param name="testMethodId">Test method ID (681 Disk, 680 MIC).</param>
    /// <param name="drugCategory">Drug category for embedded rows.</param>
    /// <param name="storedEmbeddedRows">Optional stored special rows; stored values take precedence.</param>
    /// <param name="cultureId">Culture ID for logging (optional).</param>
    /// <param name="retainStoredWhenNoMatch">When false (live criteria change via getsusceptibility), returns an empty list if no special-consideration breakpoints match. When true (load/edit), returns stored embeds so saved rows remain visible.</param>
    /// <returns>List of embedded AST rows (special considerations only).</returns>
    Task<List<ASTRowModel>> EnrichAsync(
        int organismId,
        int antibioticId,
        int guidelinesId,
        int dosage,
        int testMethodId,
        int drugCategory,
        List<ASTRowModel> storedEmbeddedRows,
        string cultureId = null,
        bool retainStoredWhenNoMatch = true);
}
