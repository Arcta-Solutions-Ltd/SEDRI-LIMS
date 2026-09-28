using arc.app.Common;
using arc.app.Config.Forms.Coding;

namespace arc.app.Config.Forms
{
    /// <summary>
    /// Factory for Coding form configurations (antibiotics, breakpoints, organisms, rule categories, etc.).
    /// </summary>
    /// <remarks>
    /// Definition names are matched case-insensitively. Returns null for unknown names.
    /// </remarks>
    internal class CodingFormFactory : IDefinitionFactory
    {
        /// <inheritdoc />
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addantibioticform" => new AddAntibioticFormConfig(),
                "addantibioticentryform" => new AddAntibioticEntryFormConfig(),
                "addantibioticgroupform" => new AddAntibioticGroupFormConfig(),
                "addbreakpointapprovalform" => new AddBreakpointApprovalFormConfig(),
                "batchapprovebreakpointform" => new BatchApproveBreakpointFormConfig(),
                "batchrejectbreakpointform" => new BatchRejectBreakpointFormConfig(),
                "addbreakpointform" => new AddBreakpointFormConfig(),
                "addcustomform" => new AddCustomFormConfig(),
                "addhostform" => new AddHostFormConfig(),
                "addlistform" => new AddListFormConfig(),
                "addorganismform" => new AddOrganismFormConfig(),
                "addresultform" => new AddResultFormConfig(),
                "addrulecategoryform" => new AddRuleCategoryFormConfig(),
                "addsourceform" => new AddSourceFormConfig(),
                "addtestpatternform" => new AddTestPatternFormConfig(),
                "addtestmethodform" => new AddTestMethodFormConfig(),
                "deleteantibioticform" => new DeleteAntibioticFormConfig(),
                "deleteantibioticentryform" => new DeleteAntibioticEntryFormConfig(),
                "deleteantibioticgroupform" => new DeleteAntibioticGroupFormConfig(),
                "deletebreakpointform" => new DeleteBreakpointFormConfig(),
                "deletelistform" => new DeleteListFormConfig(),
                "deletehostform" => new DeleteHostFormConfig(),
                "deleteorganismform" => new DeleteOrganismFormConfig(),
                "deleteresultform" => new DeleteResultFormConfig(),
                "deleterulecategoryform" => new DeleteRuleCategoryFormConfig(),
                "deletesourceform" => new DeleteSourceFormConfig(),
                "deletetestpatternform" => new DeleteTestPatternFormConfig(),
                "deletetestmethodform" => new DeleteTestMethodFormConfig(),
                "editantibioticform" => new EditAntibioticFormConfig(),
                "editbreakpointform" => new EditBreakpointFormConfig(),
                "editcustomform" => new EditCustomFormConfig(),
                "edithostform" => new EditHostFormConfig(),
                "editresultform" => new EditResultFormConfig(),
                "editrulecategoryform" => new EditRuleCategoryFormConfig(),
                "edittestpatternform" => new EditTestPatternFormConfig(),
                "edittestmethodform" => new EditTestMethodFormConfig(),
                "synonymform" => new SynonymFormConfig(),
                _ => null,
            };
        }
    }
}
