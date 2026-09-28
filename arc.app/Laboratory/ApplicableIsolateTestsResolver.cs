using arc.app.Coding;
using arc.common.Models.Laboratory;
using arc.common.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Resolves which isolate test form names are applicable for a given culture type and organism.
/// Encapsulates culture type and organism scope filtering logic in one place.
/// </summary>
public class ApplicableIsolateTestsResolver : IApplicableIsolateTestsResolver
{
    private readonly IOrganismRepository _organismRepository;
    private readonly ILogger<ApplicableIsolateTestsResolver> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicableIsolateTestsResolver"/> class.
    /// </summary>
    /// <param name="organismRepository">The organism repository for organism scope matching.</param>
    /// <param name="logger">The logger used to record which configuration branch resolved the isolate tests.</param>
    public ApplicableIsolateTestsResolver(IOrganismRepository organismRepository, ILogger<ApplicableIsolateTestsResolver> logger)
    {
        _organismRepository = organismRepository ?? throw new ArgumentNullException(nameof(organismRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<string>> GetApplicableIsolateTestsForCultureAsync(
        IReadOnlyList<LaboratoryConfigsModel> labConfigs,
        int cultureTypeId,
        int organismId,
        int orgGroupCodingId)
    {
        var cultureTypeFormNames = GetCultureTypeFormNames(labConfigs, cultureTypeId);

        if (cultureTypeFormNames.Count == 0)
        {
            _logger.LogDebug(
                "ApplicableIsolateTestsResolver found no culture type isolate test config for CultureTypeId={CultureTypeId}; falling back to organism scope only for OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}.",
                cultureTypeId,
                organismId,
                orgGroupCodingId);

            return await ResolveWhenNoCultureTypeConfigAsync(labConfigs, organismId, orgGroupCodingId);
        }

        if (organismId <= 0 && orgGroupCodingId <= 0)
        {
            _logger.LogDebug(
                "ApplicableIsolateTestsResolver resolved by culture type only for CultureTypeId={CultureTypeId} because no organism is set. CultureTypeTestCount={CultureTypeTestCount}.",
                cultureTypeId,
                cultureTypeFormNames.Count);

            return cultureTypeFormNames.ToList();
        }

        var organismScopeFormNames = await GetOrganismScopeFormNamesAsync(labConfigs, organismId, orgGroupCodingId);

        if (organismScopeFormNames.Count == 0)
        {
            _logger.LogDebug(
                "ApplicableIsolateTestsResolver resolved by culture type only for CultureTypeId={CultureTypeId} because no organism scope config matched OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. CultureTypeTestCount={CultureTypeTestCount}.",
                cultureTypeId,
                organismId,
                orgGroupCodingId,
                cultureTypeFormNames.Count);

            return cultureTypeFormNames.ToList();
        }

        var intersectedFormNames = cultureTypeFormNames
            .Where(f => organismScopeFormNames.Contains(f))
            .ToList();

        _logger.LogDebug(
            "ApplicableIsolateTestsResolver intersected culture type and organism scope config for CultureTypeId={CultureTypeId}, OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. CultureTypeTestCount={CultureTypeTestCount}, OrganismScopeTestCount={OrganismScopeTestCount}, ResolvedTestCount={ResolvedTestCount}.",
            cultureTypeId,
            organismId,
            orgGroupCodingId,
            cultureTypeFormNames.Count,
            organismScopeFormNames.Count,
            intersectedFormNames.Count);

        return intersectedFormNames;
    }

    /// <summary>
    /// Handles the case when no culture type config exists. When organism is set, still apply organism scope if configs exist.
    /// </summary>
    private async Task<IReadOnlyList<string>> ResolveWhenNoCultureTypeConfigAsync(
        IReadOnlyList<LaboratoryConfigsModel> labConfigs,
        int organismId,
        int orgGroupCodingId)
    {
        if (organismId <= 0 && orgGroupCodingId <= 0)
        {
            return null;
        }

        var organismScopeFormNames = await GetOrganismScopeFormNamesAsync(labConfigs, organismId, orgGroupCodingId);

        if (organismScopeFormNames.Count == 0)
        {
            return null;
        }

        _logger.LogDebug(
            "ApplicableIsolateTestsResolver resolved by organism scope only for OrganismId={OrganismId}, OrgGroupCodingId={OrgGroupCodingId}. OrganismScopeTestCount={OrganismScopeTestCount}.",
            organismId,
            orgGroupCodingId,
            organismScopeFormNames.Count);

        return organismScopeFormNames.ToList();
    }

    /// <summary>
    /// Returns form names from culture type configs for the given culture type.
    /// </summary>
    private static HashSet<string> GetCultureTypeFormNames(IReadOnlyList<LaboratoryConfigsModel> labConfigs, int cultureTypeId)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var cultureTypeConfigs = labConfigs
            .Where(c => c.ConfigName == "culturetypeculturetestoption")
            .ToList();

        foreach (var config in cultureTypeConfigs)
        {
            var details = ArcJson.Deserialize<LaboratoryDetailsModel>(config.Contents);
            if (details == null || details.GroupId != cultureTypeId.ToString()) continue;
            if (string.IsNullOrEmpty(details.AssociatedListId)) continue;
            foreach (var name in details.AssociatedListId.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(name.Trim());
            }
        }

        return result;
    }

    /// <summary>
    /// Returns form names from organism scope configs that match the given organism.
    /// </summary>
    private async Task<HashSet<string>> GetOrganismScopeFormNamesAsync(
        IReadOnlyList<LaboratoryConfigsModel> labConfigs,
        int organismId,
        int orgGroupCodingId)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var organismScopeConfigs = labConfigs
            .Where(c => c.ConfigName == "organismscopeculturetestoption")
            .ToList();

        foreach (var config in organismScopeConfigs)
        {
            var scope = ArcJson.Deserialize<OrganismScopeCultureTestOptionModel>(config.Contents);
            if (scope == null || !await _organismRepository.IsOrganismInScopeAsync(organismId, orgGroupCodingId, scope))
            {
                continue;
            }
            if (string.IsNullOrEmpty(scope.AssociatedListId)) continue;
            foreach (var name in scope.AssociatedListId.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(name.Trim());
            }
        }

        return result;
    }
}
