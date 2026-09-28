using arc.app.Common;

namespace arc.app.Config.UIEvents.ExpertRules;

/// <summary>
/// Factory class for creating Expert Rule UI event definitions.
/// </summary>
internal class ExpertRuleUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate UI event definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the UI event definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addexpertruleapprovaluievent" => new AddExpertRuleApprovalUIEventConfig(),
            "batchapproveexpertruleuievent" => new BatchApproveExpertRuleUIEventConfig(),
            "batchrejectexpertruleuievent" => new BatchRejectExpertRuleUIEventConfig(),
            _ => null,
        };
    }
}
