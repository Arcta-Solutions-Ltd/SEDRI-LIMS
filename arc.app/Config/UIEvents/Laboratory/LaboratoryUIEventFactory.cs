using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class for creating laboratory UI event configurations.
/// </summary>
internal class LaboratoryUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a specific UI event configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the UI event configuration to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> object representing the desired UI event configuration, 
    /// or <c>null</c> if the definition name does not match any known configurations.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetypecategoryuievent" => new AddCultureTypeCategoryUIEventConfig(),
            "addculturetypeculturetestoptionuievent" => new AddCultureTypeCultureTestOptionUIEventConfig(),
            "addorganismscopeculturetestoptionuievent" => new AddOrganismScopeCultureTestOptionUIEventConfig(),
            "addlaboratoryuievent" => new AddLaboratoryUIEventConfig(),
            "addtestcategoryuievent" => new AddTestCategoryUIEventConfig(),
            "addformspecimentypeoptionuievent" => new AddFormSpecimenTypeOptionUIEventConfig(),
            "addspecimentypeculturetypeoptionuievent" => new AddSpecimenTypeCultureTypeOptionUIEventConfig(),
            "addspecimentypedirecttestoptionuievent" => new AddSpecimenTypeDirectTestOptionUIEventConfig(),
            "addspecimentypeworkflowuievent" => new AddSpecimenTypeWorkflowUIEventConfig(),
            "deleteculturetypecategoryuievent" => new DeleteCultureTypeCategoryUIEventConfig(),
            "deleteculturetypeculturetestoptionuievent" => new DeleteCultureTypeCultureTestOptionUIEventConfig(),
            "deleteorganismscopeculturetestoptionuievent" => new DeleteOrganismScopeCultureTestOptionUIEventConfig(),
            "deletelaboratoryuievent" => new DeleteLaboratoryUIEventConfig(),
            "deleteformspecimentypeoptionuievent" => new DeleteFormSpecimenTypeOptionUIEventConfig(),
            "deletespecimentypeculturetypeoptionuievent" => new DeleteSpecimenTypeCultureTypeOptionUIEventConfig(),
            "deletespecimentypedirecttestoptionuievent" => new DeleteSpecimenTypeDirectTestOptionUIEventConfig(),
            "deletespecimentypeworkflowuievent" => new DeleteSpecimenTypeWorkflowUIEventConfig(),
            "deletetestcategoryuievent" => new DeleteTestCategoryUIEventConfig(),
            "editculturetypecategoryuievent" => new EditCultureTypeCategoryUIEventConfig(),
            "editculturetypeculturetestoptionuievent" => new EditCultureTypeCultureTestOptionUIEventConfig(),
            "editorganismscopeculturetestoptionuievent" => new EditOrganismScopeCultureTestOptionUIEventConfig(),
            "editlaboratoryuievent" => new EditLaboratoryUIEventConfig(),
            "editformspecimentypeoptionuievent" => new EditFormSpecimenTypeOptionUIEventConfig(),
            "editspecimentypeculturetypeoptionuievent" => new EditSpecimenTypeCultureTypeOptionUIEventConfig(),
            "editspecimentypedirecttestoptionuievent" => new EditSpecimenTypeDirectTestOptionUIEventConfig(),
            "editspecimentypeworkflowuievent" => new EditSpecimenTypeWorkflowUIEventConfig(),
            "edittestcategoryuievent" => new EditTestCategoryUIEventConfig(),
            "viewlaboratoryrecorduievent" => new ViewLaboratoryRecordUIEventConfig(),
            "turnaroundtimeuievent" => new TurnAroundTimeUIEventConfig(),
            "addturnaroundtimerangeuievent" => new AddTurnAroundTimeRangeUIEventConfig(),
            "editturnaroundtimerangeuievent" => new EditTurnAroundTimeRangeUIEventConfig(),
            "adddirecttestoverrideuievent" => new AddDirectTestOverrideUIEventConfig(),
            "editdirecttestoverrideuievent" => new EditDirectTestOverrideUIEventConfig(),
            "addculturetestoverrideuievent" => new AddCultureTestOverrideUIEventConfig(),
            "editculturetestoverrideuievent" => new EditCultureTestOverrideUIEventConfig(),
            _ => null,
        };
    }
}
