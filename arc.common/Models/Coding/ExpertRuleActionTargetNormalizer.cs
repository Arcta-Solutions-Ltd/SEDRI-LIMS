namespace arc.common.Models.Coding;

/// <summary>
/// Normalizes expert rule action antibiotic vs antibiotic group targets so at most one is stored.
/// </summary>
public static class ExpertRuleActionTargetNormalizer
{
    /// <summary>
    /// Result of normalizing action target ids.
    /// </summary>
    public readonly struct NormalizedActionTarget
    {
        /// <summary>
        /// Gets the normalized antibiotic id, or null when unset or cleared by mutual exclusivity.
        /// </summary>
        public int? AntibioticId { get; init; }

        /// <summary>
        /// Gets the normalized antibiotic group id, or null when unset.
        /// </summary>
        public int? AntibioticGroupId { get; init; }

        /// <summary>
        /// Gets whether both incoming ids were set and antibiotic was cleared (group wins).
        /// </summary>
        public bool CorrectedDualTarget { get; init; }
    }

    /// <summary>
    /// Applies NullIfZero and mutual exclusivity: when both targets are set, antibiotic group wins.
    /// </summary>
    /// <param name="antibioticId">Raw antibiotic id from form or grid payload.</param>
    /// <param name="antibioticGroupId">Raw antibiotic group id from form or grid payload.</param>
    /// <returns>Normalized ids safe to persist.</returns>
    public static NormalizedActionTarget Normalize(int? antibioticId, int? antibioticGroupId)
    {
        var normalizedAntibiotic = NullIfZero(antibioticId);
        var normalizedGroup = NullIfZero(antibioticGroupId);

        if (normalizedAntibiotic.HasValue && normalizedGroup.HasValue)
        {
            return new NormalizedActionTarget
            {
                AntibioticId = null,
                AntibioticGroupId = normalizedGroup,
                CorrectedDualTarget = true
            };
        }

        return new NormalizedActionTarget
        {
            AntibioticId = normalizedAntibiotic,
            AntibioticGroupId = normalizedGroup,
            CorrectedDualTarget = false
        };
    }

    /// <summary>
    /// Applies target normalization to a wizard grid action row in place.
    /// </summary>
    /// <param name="action">Action grid row to normalize.</param>
    /// <returns>True when a dual-target payload was corrected.</returns>
    public static bool NormalizeGridRow(RuleActionGridModel action)
    {
        if (action == null)
        {
            return false;
        }

        var normalized = Normalize(action.AntibioticId, action.AntibioticGroupId);
        action.AntibioticId = normalized.AntibioticId;
        action.AntibioticGroupId = normalized.AntibioticGroupId;
        return normalized.CorrectedDualTarget;
    }

    private static int? NullIfZero(int? value) => value is > 0 ? value : null;
}
