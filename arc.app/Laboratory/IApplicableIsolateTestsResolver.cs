using arc.common.Models.Laboratory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Resolves which isolate test form names are applicable for a given culture type and organism
/// based on laboratory configuration (culture type and organism scope configs).
/// </summary>
public interface IApplicableIsolateTestsResolver
{
    /// <summary>
    /// Returns the list of isolate test form names applicable for the given culture type and organism.
    /// Filters by culture type first (when configured), then by organism scope when an organism is set.
    /// When no culture type config exists but organism scope configs match, returns organism-scope-restricted list.
    /// </summary>
    /// <param name="labConfigs">Laboratory configs for the lab (already filtered by laboratory ID).</param>
    /// <param name="cultureTypeId">The culture type ID.</param>
    /// <param name="organismId">The organism ID (0 if not set).</param>
    /// <param name="orgGroupCodingId">The organism group coding ID (0 if using specific organism).</param>
    /// <returns>List of applicable isolate test form names, or null when frontend should use its fallback.</returns>
    Task<IReadOnlyList<string>> GetApplicableIsolateTestsForCultureAsync(
        IReadOnlyList<LaboratoryConfigsModel> labConfigs,
        int cultureTypeId,
        int organismId,
        int orgGroupCodingId);
}
