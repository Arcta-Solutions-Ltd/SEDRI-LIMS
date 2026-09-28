using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class for creating laboratory query configurations.
/// </summary>
internal class LaboratoryQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a specific query configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the query configuration to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> object representing the desired query configuration,
    /// or <c>null</c> if the definition name does not match any known configurations.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "culturetypecategorisationlistquery" => new CultureTypeCategorisationListQuery(),
            "culturetypeculturetestoptionlistquery" => new CultureTypeCultureTestOptionListQuery(),
            "editorganismscopeculturetestoptionquery" => new EditorOrganismScopeCultureTestOptionQuery(),
            "editorganismscopeculturetestlistquery" => new OrganismScopeCultureTestOptionListQuery(),
            "organismscopeculturetestlistquery" => new OrganismScopeCultureTestOptionListQuery(),
            "organismscopeculturetestoptionlistquery" => new OrganismScopeCultureTestOptionListQuery(),
            "editspecimentypeworkflowquery" => new EditSpecimenTypeWorkflowQuery(),
            "laboratorybyid" => new LaboratoryByIdQuery(),
            "laboratorycount" => new LaboratoryCountQuery(),
            "laboratoryforlaboratoryviewquery" => new LaboratoryForLaboratoryViewQuery(),
            "laboratorylist" => new LaboratoryListQuery(),
            "laboratoryspecimencount" => new LaboratorySpecimenCountQuery(),
            "laboratoryusercount" => new LaboratoryUserCountQuery(),
            "singlelaboratoryforlaboratorylist" => new SingleLaboratoryForLaboratoryListQuery(),
            "formspecimentypeoptionlistquery" => new FormSpecimenTypeOptionListQuery(),
            "editformspecimentypeoptionquery" => new EditFormSpecimenTypeOptionQuery(),
            "specimentypeculturetypeoptionlistquery" => new SpecimenTypeCultureTypeOptionListQuery(),
            "specimentypedirecttestoptionlistquery" => new SpecimenTypeDirectTestOptionListQuery(),
            "specimentypeworkflowlistquery" => new SpecimenTypeWorkflowListQuery(),
            "turnaroundtimeforminitialquery" => new TurnAroundTimeFormInitialQuery(),
            _ => null,
        };
    }
}
