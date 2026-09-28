using arc.app.Common;
using arc.app.Config.Queries.Coding;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Factory that creates query definitions for coding-related operations.
    /// Supplies the appropriate <see cref="IDefinition"/> (query config) for a given definition name,
    /// covering antibiotics, breakpoints, organisms, hosts, results, test patterns, and related coding entities.
    /// </summary>
    /// <remarks>
    /// Definition names are matched case-insensitively. Returns null for unknown names.
    /// </remarks>
    internal class CodingQueryFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates a query definition instance for the given definition name.
        /// </summary>
        /// <param name="definitionName">The query definition name (case-insensitive).</param>
        /// <returns>The corresponding query definition, or null if not found.</returns>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                // Antibiotic queries
                "antibioticbyalreadyexistsforadd" => new AntibioticAlreadyExistsForAddQuery(),
                "antibioticbyalreadyexistsforedit" => new AntibioticAlreadyExistsForEditQuery(),
                "antibioticbyidforeditquery" => new AntibioticByIdForEditQuery(),
                "antibioticcodingexistsquery" => new AntibioticCodingExistsQuery(),
                "antibioticentrybyidquery" => new AntibioticEntryByIdQuery(),
                "antibioticexistsquery" => new AntibioticExistsQuery(),
                "antibioticgroupexistsquery" => new AntibioticGroupExistsQuery(),
                "antibioticinastquery" => new AntibioticInAstQuery(),
                "antibioticinbreakpointquery" => new AntibioticInBreakpointQuery(),
                "antibioticintestpatternlinequery" => new AntibioticInTestPatternLineQuery(),
                "antibioticlist" => new AntibioticListQuery(),

                // Breakpoint queries
                "breakpointapprovalforminitialquery" => new BreakpointApprovalFormInitialQuery(),
                "breakpointapprovallistbybreakpointid" => new BreakpointApprovalListByBreakpointIdQuery(),
                "breakpointbyidforedit" => new BreakpointByIdForEditQuery(),
                "breakpointlinelistbybreakpointid" => new BreakpointLineListByBreakpointIdQuery(),
                "breakpointlist" => new BreakpointListQuery(),
                "breakpointviewquery" => new BreakpointViewQuery(),

                // Validation and duplicate-check queries
                "checkduplicatecustomentry" => new CheckDuplicateCustomEntryQuery(),
                "checkduplicatecustomentrycode" => new CheckDuplicateCustomEntryCodeQuery(),
                "checkwhethercodinglistisempty" => new CheckWhetherCodingListIsEmptyQuery(),
                "checkwhethersourcecontainsbreakpoints" => new CheckWhetherSourceContainsBreakpointsQuery(),
                "checkwhethersourcecontainsexpertrules" => new CheckWhetherSourceContainsExpertRulesQuery(),
                "codinglistitemexists" => new CodingListItemExistsQuery(),
                "sourcelistitemexists" => new SourceListItemExistsQuery(),

                // Custom entry and edit queries
                "customentrybyorganismid" => new CustomEntryByOrganismIdQuery(),
                "deleteexpertrule" => new DeleteExpertRuleQuery(),
                "deleteexpertruleconditionquery" => new DeleteExpertRuleConditionQuery(),
                "deleteexpertruleactionquery" => new DeleteExpertRuleActionQuery(),
                "deleteexpertruletestconditionquery" => new DeleteExpertRuleTestConditionQuery(),
                "editbreakpoint" => new EditBreakpointQuery(),
                "editexpertrulequery" => new EditExpertRuleQuery(),
                "editexpertruleconditionquery" => new EditExpertRuleConditionQuery(),
                "editexpertruleactionquery" => new EditExpertRuleActionQuery(),
                "editexpertruletestconditionquery" => new EditExpertRuleTestConditionQuery(),
                "edittestpattern" => new EditTestPatternQuery(),

                "expertrulelist" => new ExpertRuleListQuery(),
                "expertruleviewquery" => new ExpertRuleViewQuery(),
                "expertruleconditionlistbyexpertruleid" => new ExpertRuleConditionListByExpertRuleIdQuery(),
                "expertruletestconditionlistbyexpertruleid" => new ExpertRuleTestConditionListByExpertRuleIdQuery(),
                "expertruleactionlistbyexpertruleid" => new ExpertRuleActionListByExpertRuleIdQuery(),
                //"expertruleactionsquery" => new ExpertRuleActionsQuery(),

                // Taxonomic / organism hierarchy lists
                "familylist" => new FamilyListQuery(),
                "genuslist" => new GenusListQuery(),
                "orderandfamilyfromgenusid" => new OrderAndFamilyFromGenusIdQuery(),
                "orderlist" => new OrderListQuery(),
                "serotypelist" => new SerotypeListQuery(),
                "specieslist" => new SpeciesListQuery(),
                "subspecieslist" => new SubSpeciesListQuery(),

                // Host queries
                "hostexists" => new HostExistsQuery(),
                "hostexistsinbreakpoint" => new HostExistsInBreakpointQuery(),

                // Organism queries
                "organismcodingexists" => new OrganismCodingExistsQuery(),
                "organismculturecount" => new OrganismCultureCountQuery(),
                "organismexists" => new OrganismExistsQuery(),
                "organismlist" => new OrganismListQuery(),
                "organismlistentrybyid" => new OrganismListEntryByIdQuery(),
                "organismsearch" => new OrganismSearchQuery(),

                "resultexists" => new ResultExistsQuery(),
                "singletagfortaglist" => new SingleTagForTagListQueryConfig(),
                "resultexistsinbreakpoint" => new ResultExistsInBreakpointQuery(),
                "rulecategoryexists" => new RuleCategoryExistsQuery(),
                "rulecategoryinuse" => new RuleCategoryInUseQuery(),
                "singleexpertruleforexpertrulelist" => new SingleExpertRuleForExpertRuleListQuery(),
                "singleorganismfororganismlist" => new SingleOrganismForOrganismListQuery(),

                "specimenorganism" => new SpecimenOrganismQuery(),
                "specimenorganismcode" => new SpecimenOrganismCodeQuery(),
                "synonymsfororganismquery" => new SynonymsForOrganismQuery(),

                // Result and test method queries
                "testmethodexists" => new TestMethodExistsQuery(),
                "testmethodexistsinbreakpoint" => new TestMethodExistsInBreakpointQuery(),

                // Test pattern queries
                "testpatternbyid" => new TestPatternByIdQuery(),
                "taglist" => new TagListQueryConfig(),
                "taghaschildren" => new TagHasChildrenQuery(),
                "testpatternlist" => new TestPatternListQuery(),

                _ => null,
            };
        }
    }
}
