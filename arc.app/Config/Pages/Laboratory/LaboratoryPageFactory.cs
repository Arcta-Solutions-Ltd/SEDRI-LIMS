using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Factory class for creating laboratory page configurations.
/// </summary>
internal class LaboratoryPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a specific page configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the page configuration to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> object representing the desired page configuration, 
    /// or <c>null</c> if the definition name does not match any known configurations.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetypecategorypage" => new AddCultureTypeCategoryPageConfig(),
            "addspecimentypeworkflowpage" => new AddSpecimenTypeWorkflowPageConfig(),
            "addlaboratorypage" => new AddLaboratoryPageConfig(),
            "addtestcategorypage" => new AddTestCategoryPageConfig(),
            "deleteculturetypecategorypage" => new DeleteCultureTypeCategoryPageConfig(),
            "deletespecimentypeworkflowpage" => new DeleteSpecimenTypeWorkflowPageConfig(),
            "deletelaboratorypage" => new DeleteLaboratoryPageConfig(),
            "deletetestcategorypage" => new DeleteTestCategoryPageConfig(),
            "editculturetypecategorypage" => new EditCultureTypeCategoryPageConfig(),
            "editspecimentypeworkflowpage" => new EditSpecimenTypeWorkflowPageConfig(),
            "editlaboratorypage" => new EditLaboratoryPageConfig(),
            "edittestcategorypage" => new EditTestCategoryPageConfig(),
            "turnaroundtimepage" => new TurnAroundTimePageConfig(),
            "addturnaroundtimerangepage" => new AddTurnAroundTimeRangePageConfig(),
            "adddirecttestoverridepage" => new AddDirectTestOverridePageConfig(),
            "addculturetestoverridepage" => new AddCultureTestOverridePageConfig(),
            _ => null,
        };
    }
}
