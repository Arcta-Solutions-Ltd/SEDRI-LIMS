using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class for creating Alert UI event definitions.
/// </summary>
internal class AlertUIEventFactory : IDefinitionFactory
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
            "addalertapprovaluievent" => new AddAlertApprovalUIEventConfig(),
            "addalertuievent" => new AddAlertUIEventConfig(),
            "addalertcategoryuievent" => new AddAlertCategoryUIEventConfig(),
            "addorganismalertuievent" => new AddOrganismAlertUIEventConfig(),
            "addtaguievent" => new AddTagUIEventConfig(),
            "deletealertuievent" => new DeleteAlertUIEventConfig(),
            "deletealertcategoryuievent" => new DeleteAlertCategoryUIEventConfig(),
            "deletetaguievent" => new DeleteTagUIEventConfig(),
            "editalertuievent" => new EditAlertUIEventConfig(),
            "viewalertuievent" => new ViewAlertUIEventConfig(),
            "editalertcategoryuievent" => new EditAlertCategoryUIEventConfig(),
            "edittaguievent" => new EditTagUIEventConfig(),
            _ => null,
        };
    }
}

