using arc.common.Models.AST;

using System.Collections.Generic;

using System.Linq;



namespace arc.data.Coding;



/// <summary>

/// Filters expert rule actions to those allowed by a laboratory's selected antibiotic groups.

/// </summary>

public static class ExpertRuleLaboratoryAntibioticFilter

{

    /// <summary>

    /// Keeps actions whose target antibiotic belongs to an allowed group listitem id, or whose target group listitem id

    /// is allowed. Alert-only actions (no antibiotic or group) are always kept.

    /// </summary>

    /// <param name="actions">Expert rule actions to filter.</param>

    /// <param name="allowedAntibioticGroupIds">Allowed antibiotic group listitem ids for the laboratory.</param>

    /// <param name="antibioticIdToCodingIds">Maps <c>antibiotic.id</c> to group listitem ids from <c>antibioticcoding</c>.</param>

    public static List<ExpertRuleActionReturnModel> FilterByAllowedAntibioticGroups(

        IReadOnlyList<ExpertRuleActionReturnModel> actions,

        HashSet<int> allowedAntibioticGroupIds,

        IReadOnlyDictionary<int, HashSet<int>> antibioticIdToCodingIds)

    {

        if (actions == null || actions.Count == 0)

        {

            return [];

        }



        var filtered = new List<ExpertRuleActionReturnModel>();

        foreach (var a in actions)

        {

            if (IsAlertOnlyExpertRuleAction(a))

            {

                filtered.Add(a);

                continue;

            }



            if (a.AntibioticId > 0)

            {

                if (antibioticIdToCodingIds != null

                    && antibioticIdToCodingIds.TryGetValue(a.AntibioticId, out var codingIds)

                    && codingIds.Any(id => allowedAntibioticGroupIds.Contains(id)))

                {

                    filtered.Add(a);

                }



                continue;

            }



            if (a.AntibioticGroupId.HasValue

                && a.AntibioticGroupId.Value > 0

                && allowedAntibioticGroupIds.Contains(a.AntibioticGroupId.Value))

            {

                filtered.Add(a);

            }

        }



        return filtered;

    }



    private static bool IsAlertOnlyExpertRuleAction(ExpertRuleActionReturnModel a) =>

        a.AntibioticId <= 0 && (!a.AntibioticGroupId.HasValue || a.AntibioticGroupId.Value <= 0);

}

