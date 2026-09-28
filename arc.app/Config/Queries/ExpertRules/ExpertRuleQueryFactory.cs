using arc.app.Common;

namespace arc.app.Config.Queries.ExpertRules;

/// <summary>
/// Factory that creates query definitions for expert rule-related operations.
/// </summary>
internal class ExpertRuleQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a query definition instance for the given definition name.
    /// </summary>
    /// <param name="definitionName">The query definition name (case-insensitive).</param>
    /// <returns>The corresponding query definition, or null if not found.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "expertruleapprovalforminitialquery" => new ExpertRuleApprovalFormInitialQuery(),
            "expertruleapprovallistbyexpertruleid" => new ExpertRuleApprovalListByExpertRuleIdQuery(),
            _ => null,
        };
    }
}
