using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class for creating laboratory form configuration instances.
/// </summary>
internal class LaboratoryFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a laboratory form configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the laboratory form configuration to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name,
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addlaboratoryform" => new AddLaboratoryFormConfig(),
            "addculturetypeculturetestoptionform" => new AddCultureTypeCultureTestOptionFormConfig(),
            "addorganismscopeculturetestoptionform" => new AddOrganismScopeCultureTestOptionFormConfig(),
            "addculturetypecategoryform" => new AddCultureTypeCategoryFormConfig(),
            "addformspecimentypeoptionform" => new AddFormSpecimenTypeOptionFormConfig(),
            "addspecimentypeculturetypeoptionform" => new AddSpecimenTypeCultureTypeOptionFormConfig(),
            "addspecimentypedirecttestoptionform" => new AddSpecimenTypeDirectTestOptionFormConfig(),
            "addspecimentypeworkflowform" => new AddSpecimenTypeWorkflowFormConfig(),
            "addtestcategoryform" => new AddTestCategoryFormConfig(),
            "deleteculturetypecategoryform" => new DeleteCultureTypeCategoryFormConfig(),
            "deletelaboratoryform" => new DeleteLaboratoryFormConfig(),
            "deleteculturetypeculturetestoptionform" => new DeleteCultureTypeCultureTestOptionFormConfig(),
            "deleteorganismscopeculturetestoptionform" => new DeleteOrganismScopeCultureTestOptionFormConfig(),
            "deleteformspecimentypeoptionform" => new DeleteFormSpecimenTypeOptionFormConfig(),
            "deletespecimentypeculturetypeoptionform" => new DeleteSpecimenTypeCultureTypeOptionFormConfig(),
            "deletespecimentypedirecttestoptionform" => new DeleteSpecimenTypeDirectTestOptionFormConfig(),
            "deletespecimentypeworkflowform" => new DeleteSpecimenTypeWorkflowFormConfig(),
            "deletetestcategoryform" => new DeleteTestCategoryFormConfig(),
            "editculturetypecategoryform" => new EditCultureTypeCategoryFormConfig(),
            "editlaboratoryform" => new EditLaboratoryFormConfig(),
            "editculturetypeculturetestoptionform" => new EditCultureTypeCultureTestOptionFormConfig(),
            "editorganismscopeculturetestoptionform" => new EditOrganismScopeCultureTestOptionFormConfig(),
            "editformspecimentypeoptionform" => new EditFormSpecimenTypeOptionFormConfig(),
            "editspecimentypeculturetypeoptionform" => new EditSpecimenTypeCultureTypeOptionFormConfig(),
            "editspecimentypedirecttestoptionform" => new EditSpecimenTypeDirectTestOptionFormConfig(),
            "editspecimentypeworkflowform" => new EditSpecimenTypeWorkflowFormConfig(),
            "edittestcategoryform" => new EditTestCategoryFormConfig(),
            "turnaroundtimeform" => new TurnAroundTimeFormConfig(),
            "addturnaroundtimerangeform" => new AddTurnAroundTimeRangeFormConfig(),
            "editturnaroundtimerangeform" => new EditTurnAroundTimeRangeFormConfig(),
            "adddirecttestoverrideform" => new AddDirectTestOverrideFormConfig(),
            "editdirecttestoverrideform" => new EditDirectTestOverrideFormConfig(),
            "addculturetestoverrideform" => new AddCultureTestOverrideFormConfig(),
            "editculturetestoverrideform" => new EditCultureTestOverrideFormConfig(),
            _ => null,
        };
    }
}
