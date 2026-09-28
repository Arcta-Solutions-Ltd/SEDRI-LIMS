using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class QualityFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addiqctestform" => new AddIqcTestFormConfig(),
                "addiqctestprofileform" => new AddIqcTestProfileFormConfig(),
                "deleteiqcresultform" => new DeleteIqcResultFormConfig(),
                "deleteiqctestform" => new DeleteIqcTestFormConfig(),
                "deleteiqctestprofileform" => new DeleteIqcTestProfileFormConfig(),
                "editiqcresultform" => new EditIqcResultForm(),
                "editiqctestprofileqcorganismform" => new EditIqcTestProfileQcOrganismForm(),
                "editiqctestqcorganismsform" => new EditIqcTestQcOrganismsFormConfig(),
                "markiqctestcompleteform" => new MarkIqcTestCompleteFormConfig(),
                "runiqctestform" => new RunIqcTestFormConfig(),
                _ => null,
            };
        }
    }
}
