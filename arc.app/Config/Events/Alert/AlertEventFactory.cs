using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Factory class for creating Alert Event definitions.
/// </summary>
internal class AlertEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate event definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the event definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addalert" => new AddAlertEventConfig(),
            "addalertapproval" => new AddAlertApprovalEventConfig(),
            "addalertcategory" => new AddAlertCategoryEventConfig(),
            "addorganismalert" => new AddOrganismAlertEventConfig(),
            "addtag" => new AddTagEventConfig(),
            "deletealert" => new DeleteAlertEventConfig(),
            "deletealertcategory" => new DeleteAlertCategoryEventConfig(),
            "deletetag" => new DeleteTagEventConfig(),
            "editalert" => new EditAlertEventConfig(),
            "editalertcategory" => new EditAlertCategoryEventConfig(),
            "edittag" => new EditTagEventConfig(),
            _ => null,
        };
    }
}

