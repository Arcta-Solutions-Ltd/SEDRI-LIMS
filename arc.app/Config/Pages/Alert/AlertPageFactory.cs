using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory class for creating Alert Page definitions.
/// </summary>
internal class AlertPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate page definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the page definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addalertapprovalpage" => new AddAlertApprovalPageConfig(),
            "addalertcategorypage" => new AddAlertCategoryPageConfig(),
            "addtagpage" => new AddTagPageConfig(),
            "alertdetailspage" => new AlertDetailsPageConfig(),
            "alertdetailssecondpage" => new AlertDetailsSecondPageConfig(),
            "alerttestdetailsonlypage" => new AlertTestDetailsOnlyPageConfig(),
            "deletealertpage" => new DeleteAlertPageConfig(),
            "deletealertcategorypage" => new DeleteAlertCategoryPageConfig(),
            "deletetagpage" => new DeleteTagPageConfig(),
            "editalertcategorypage" => new EditAlertCategoryPageConfig(),
            "editalertdetailspage" => new EditAlertDetailsPageConfig(),
            "edittagpage" => new EditTagPageConfig(),
            _ => null,
        };
    }
}

