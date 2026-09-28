using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Factory class for creating laboratory event configuration instances.
/// </summary>
public class LaboratoryEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an instance of a laboratory event configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the laboratory event configuration to create.</param>
    /// <returns>
    /// An instance of <see cref="IDefinition"/> corresponding to the specified definition name,
    /// or <c>null</c> if no matching definition is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addculturetypecategoryevent" => new AddCultureTypeCategoryEventConfig(),
            "addculturetypeculturetestoptionevent" => new AddCultureTypeCultureTestOptionEventConfig(),
            "addorganismscopeculturetestoptionevent" => new AddOrganismScopeCultureTestOptionEventConfig(),
            "addlaboratory" => new AddLaboratoryEventConfig(),
            "addformspecimentypeoption" => new AddFormSpecimenTypeOptionEventConfig(),
            "addspecimentypeculturetypeoptionevent" => new AddSpecimenTypeCultureTypeOptionEventConfig(),
            "addspecimentypedirecttestoptionevent" => new AddSpecimenTypeDirectTestOptionEventConfig(),
            "addspecimentypeworkflowevent" => new AddSpecimenTypeWorkflowEventConfig(),
            "addtestcategoryevent" => new AddTestCategoryEventConfig(),
            "deleteculturetypecategoryevent" => new DeleteCultureTypeCategoryEventConfig(),
            "deleteculturetypeculturetestoptionevent" => new DeleteCultureTypeCultureTestOptionEventConfig(),
            "deleteorganismscopeculturetestoptionevent" => new DeleteOrganismScopeCultureTestOptionEventConfig(),
            "deletelaboratory" => new DeleteLaboratoryEventConfig(),
            "deleteformspecimentypeoption" => new DeleteFormSpecimenTypeOptionEventConfig(),
            "deletespecimentypeculturetypeoptionevent" => new DeleteSpecimenTypeCultureTypeOptionEventConfig(),
            "deletespecimentypedirecttestoptionevent" => new DeleteSpecimenTypeDirectTestOptionEventConfig(),
            "deletespecimentypeworkflowevent" => new DeleteSpecimenTypeWorkflowEventConfig(),
            "deletetestcategoryevent" => new DeleteTestCategoryEventConfig(),
            "editculturetypecategoryevent" => new EditCultureTypeCategoryEventConfig(),
            "editculturetypeculturetestoptionevent" => new EditCultureTypeCultureTestOptionEventConfig(),
            "editorganismscopeculturetestoptionevent" => new EditOrganismScopeCultureTestOptionEventConfig(),
            "editlaboratory" => new EditLaboratoryEventConfig(),
            "editformspecimentypeoption" => new EditFormSpecimenTypeOptionEventConfig(),
            "editspecimentypeculturetypeoptionevent" => new EditSpecimenTypeCultureTypeOptionEventConfig(),
            "editspecimentypedirecttestoptionevent" => new EditSpecimenTypeDirectTestOptionEventConfig(),
            "editspecimentypeworkflowevent" => new EditSpecimenTypeWorkflowEventConfig(),
            "edittestcategoryevent" => new EditTestCategoryEventConfig(),
            "updateturnaroundtimeconfig" => new UpdateTurnAroundTimeConfigEventConfig(),
            "addturnaroundtimerange" => new AddTurnAroundTimeRangeEventConfig(),
            "editturnaroundtimerange" => new EditTurnAroundTimeRangeEventConfig(),
            "adddirecttestoverride" => new AddDirectTestOverrideEventConfig(),
            "editdirecttestoverride" => new EditDirectTestOverrideEventConfig(),
            "addculturetestoverride" => new AddCultureTestOverrideEventConfig(),
            "editculturetestoverride" => new EditCultureTestOverrideEventConfig(),
            _ => null,
        };
    }
}
