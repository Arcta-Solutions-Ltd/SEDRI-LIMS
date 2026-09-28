using arc.common.Models.Coding;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.AST;

/// <summary>
/// Selects the default test pattern for a culture when multiple patterns match.
/// Taxonomic patterns are preferred over organism-group patterns; within each category
/// the closest taxonomic depth wins, with MakeDefault as a tie-break at the same depth.
/// </summary>
public static class TestPatternDefaultSelector
{
    /// <summary>
    /// Result of default test pattern selection, including the chosen pattern (if any) and a reason code for logging.
    /// </summary>
    public sealed class SelectionResult
    {
        public TestPatternScopeModel Selected { get; init; }
        public string Reason { get; init; }
    }

    /// <summary>
    /// Picks the default test pattern from matched options.
    /// Returns null when no unambiguous default can be determined.
    /// </summary>
    public static SelectionResult SelectDefault(IReadOnlyList<TestPatternScopeModel> options)
    {
        if (options == null || options.Count == 0)
        {
            return new SelectionResult { Selected = null, Reason = "NoOptions" };
        }

        if (options.Count == 1)
        {
            return new SelectionResult { Selected = options[0], Reason = "SingleOption" };
        }

        var taxonomic = options.Where(p => !p.IsOrganismGroupMatch).ToList();
        var taxonomicSelection = SelectByTaxonomicHierarchy(taxonomic);
        if (taxonomicSelection != null)
        {
            return new SelectionResult
            {
                Selected = taxonomicSelection,
                Reason = $"TaxonomicMatch:{GetSpecificityLabel(taxonomicSelection)}"
            };
        }

        var groupPatterns = options.Where(p => p.IsOrganismGroupMatch).ToList();
        var groupSelection = SelectAmongOrganismGroups(groupPatterns);
        if (groupSelection != null)
        {
            return new SelectionResult { Selected = groupSelection, Reason = "OrganismGroupFallback" };
        }

        return new SelectionResult { Selected = null, Reason = "Ambiguous" };
    }

    /// <summary>
    /// Walks taxonomic specificity levels from most to least specific and returns a single match when unambiguous.
    /// </summary>
    internal static TestPatternScopeModel SelectByTaxonomicHierarchy(IReadOnlyList<TestPatternScopeModel> taxonomicPatterns)
    {
        if (taxonomicPatterns == null || taxonomicPatterns.Count == 0)
        {
            return null;
        }

        if (taxonomicPatterns.Count == 1)
        {
            return taxonomicPatterns[0];
        }

        for (var level = 0; level < 8; level++)
        {
            var defaultAtLevel = taxonomicPatterns.FirstOrDefault(p => MatchesLevel(p, level) && IsMakeDefault(p));
            if (defaultAtLevel != null)
            {
                return defaultAtLevel;
            }

            var candidates = taxonomicPatterns.Where(p => MatchesLevel(p, level)).ToList();
            if (candidates.Count == 1)
            {
                return candidates[0];
            }

            if (candidates.Count > 1)
            {
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Selects among organism-group patterns when no taxonomic pattern matched.
    /// Prefers MakeDefault; returns null when multiple group patterns tie without a default.
    /// </summary>
    internal static TestPatternScopeModel SelectAmongOrganismGroups(IReadOnlyList<TestPatternScopeModel> groupPatterns)
    {
        if (groupPatterns == null || groupPatterns.Count == 0)
        {
            return null;
        }

        if (groupPatterns.Count == 1)
        {
            return groupPatterns[0];
        }

        var defaults = groupPatterns.Where(IsMakeDefault).ToList();
        if (defaults.Count == 1)
        {
            return defaults[0];
        }

        if (defaults.Count > 1)
        {
            return null;
        }

        return null;
    }

    private static bool IsMakeDefault(TestPatternScopeModel pattern) =>
        pattern.MakeDefault != null && pattern.MakeDefault.Equals("Yes", System.StringComparison.OrdinalIgnoreCase);

    private static bool MatchesLevel(TestPatternScopeModel pattern, int level) =>
        (level == 0 && pattern.SerotypeId != 0) ||
        (level == 1 && pattern.SubSpeciesId != 0) ||
        (level == 2 && pattern.SpeciesId != 0) ||
        (level == 3 && pattern.GenusId != 0) ||
        (level == 4 && pattern.FamilyId != 0) ||
        (level == 5 && pattern.OrderId != 0) ||
        (level == 6 && pattern.OrgGroupCodingId != 0) ||
        (level == 7 && pattern.AdditionalId != 0);

    private static string GetSpecificityLabel(TestPatternScopeModel pattern)
    {
        if (pattern.SerotypeId != 0) return "Serotype";
        if (pattern.SubSpeciesId != 0) return "Subspecies";
        if (pattern.SpeciesId != 0) return "Species";
        if (pattern.GenusId != 0) return "Genus";
        if (pattern.FamilyId != 0) return "Family";
        if (pattern.OrderId != 0) return "Order";
        if (pattern.AdditionalId != 0) return "Additional";
        return "Unknown";
    }
}
