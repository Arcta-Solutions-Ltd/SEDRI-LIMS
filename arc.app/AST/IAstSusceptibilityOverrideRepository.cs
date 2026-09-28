using System.Collections.Generic;
using System.Threading.Tasks;
using arc.data.model.AST;

namespace arc.app.AST;

/// <summary>
/// Persistence for manual AST susceptibility override audit rows (astsusceptibilityoverride).
/// </summary>
public interface IAstSusceptibilityOverrideRepository
{
    /// <summary>Loads all override rows for a culture.</summary>
    Task<IReadOnlyList<AstSusceptibilityOverrideDataModel>> GetByCultureIdAsync(int cultureId);

    /// <summary>Upserts override rows and deletes rows not present in the payload (revert / removed lines).</summary>
    Task SyncForCultureAsync(int cultureId, IEnumerable<AstSusceptibilityOverrideDataModel> overrides, string username);

    /// <summary>Deletes the override row for a culture and line key.</summary>
    Task DeleteByLineKeyAsync(int cultureId, string lineKey);
}
