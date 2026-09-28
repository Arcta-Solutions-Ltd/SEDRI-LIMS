using arc.app.Common;
using arc.app.Config.Mapper.Coding;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory that creates mapper definitions for coding-related operations.
/// Supplies the appropriate <see cref="IDefinition"/> (result/event mapper) for a given definition name,
/// covering antibiotics, breakpoints, organisms, hosts, results, sources, test methods, and coding lists.
/// </summary>
/// <remarks>
/// Definition names are matched case-insensitively. Returns null for unknown names.
/// </remarks>
internal class CodingMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a mapper definition instance for the given definition name.
    /// </summary>
    /// <param name="definitionName">The mapper definition name (case-insensitive).</param>
    /// <returns>The corresponding mapper definition, or null if not found.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            // Add mappers
            "addantibioticgroupmapper" => new AddAntibioticGroupMapper(),
            "addcodinglistmapper" => new AddCodingListMapper(),
            "addhostmapper" => new AddHostMapper(),
            "addresultmapper" => new AddResultMapper(),
            "addrulecategorymapper" => new AddRuleCategoryMapper(),
            "addexpertruleactionmapper" => new AddExpertRuleActionMapper(),
            "addexpertruleconditionmapper" => new AddExpertRuleConditionMapper(),
            "addexpertruletestconditionmapper" => new AddExpertRuleTestConditionMapper(),
            "addsourcemapper" => new AddSourceMapper(),
            "addtestmethodmapper" => new AddTestMethodMapper(),

            // Antibiotic mappers
            "antibioticcodingexistsmapper" => new AntibioticCodingExistsMapper(),
            "antibioticexistsmapper" => new AntibioticExistsMapper(),
            "antibioticcodingeventmapper" => new AntibioticCodingEventMapper(),
            "antibioticgroupexistsmapper" => new AntibioticGroupExistsMapper(),

            // Breakpoint mappers
            "breakpointbyidforeditresultmapping" => new BreakpointByIdForEditResultMapper(),
            "breakpointviewmapper" => new BreakpointViewMapper(),
            "expertruleviewmapper" => new ExpertRuleViewMapper(),

            // Coding list and list item mappers
            "codinglistexistsmapper" => new CodingListExistsMapper(),
            "duplicatelistitemmapper" => new DuplicateListItemMapper(),

            // Organism mappers
            "deleteorganismmapper" => new DeleteOrganismMapper(),
            "organismcodingmapper" => new OrganismCodingMapper(),
            "organismcodingeventmapper" => new OrganismCodingEventMapper(),
            "organismculturecountmapper" => new OrganismCultureCountMapper(),
            "organismexistsmapper" => new OrganismExistsMapper(),


            "editexpertruleconditionqueryresultmapper" => new EditExpertRuleConditionResultMapper(),
            "editexpertruleactionqueryresultmapper" => new EditExpertRuleActionQueryResultMapper(),
            "editexpertruletestconditionqueryresultmapper" => new EditExpertRuleTestConditionQueryResultMapper(),
            "editexpertruleconditionmapper" => new EditExpertRuleConditionMapper(),
            "editexpertruleactionmapper" => new EditExpertRuleActionMapper(),
            "editexpertruletestconditionmapper" => new EditExpertRuleTestConditionMapper(),
            "deleteexpertruleconditionquerymapper" => new DeleteExpertRuleConditionQueryMapper(),
            "deleteexpertruleactionquerymapper" => new DeleteExpertRuleActionQueryMapper(),
            "deleteexpertruletestconditionquerymapper" => new DeleteExpertRuleTestConditionQueryMapper(),
            "edithostmapper" => new EditHostMapper(),
            "editresultmapper" => new EditResultMapper(),
            "edittestmethodmapper" => new EditTestMethodMapper(),
            "editrulecategorymapper" => new EditRuleCategoryMapper(),

            // Host mappers
            "hostexistsmapper" => new HostExistsMapper(),
            "hostexistsinbreakpointmapper" => new HostExistsInBreakpointMapper(),

            // Source mappers
            "idtosourceidmapper" => new IdToSourceIdMapper(),
            "sourcelistexistsmapper" => new SourceListExistsMapper(),

            // Result and test method existence mappers
            "resultexistsmapper" => new ResultExistsMapper(),
            "resultexistsinbreakpointmapper" => new ResultExistsInBreakpointMapper(),

            "rulecategoryexistsmapper" => new RuleCategoryExistsMapper(),
            "rulecategoryinusemapper" => new RuleCategoryInUseMapper(),

            "testmethodexistsmapper" => new TestMethodExistsMapper(),
            "testmethodexistsinbreakpointmapper" => new TestMethodExistsInBreakpointMapper(),

            _ => null,
        };
    }
}
