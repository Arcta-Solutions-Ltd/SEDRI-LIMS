using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class for creating Alert Form definitions.
/// </summary>
internal class AlertFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate form definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the form definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addalertapprovalform" => new AddAlertApprovalFormConfig(),
            "addalertform" => new AddAlertFormConfig(),
            "addalertcategoryform" => new AddAlertCategoryFormConfig(),
            "addorganismalertform" => new AddOrganismAlertFormConfig(),
            "addtagform" => new AddTagFormConfig(),
            "deletealertform" => new DeleteAlertFormConfig(),
            "deletealertcategoryform" => new DeleteAlertCategoryFormConfig(),
            "deletetagform" => new DeleteTagFormConfig(),
            "editalertform" => new EditAlertFormConfig(),
            "editalertcategoryform" => new EditAlertCategoryFormConfig(),
            "edittagform" => new EditTagFormConfig(),
            _ => null,
        };
    }
}

