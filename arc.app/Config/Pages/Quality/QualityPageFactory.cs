using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class QualityPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addiqctestprofilepage" => new AddIqcTestProfilePageConfig(),
                "deleteiqcresultpage" => new DeleteIqcResultPageConfig(),
                "deleteiqctestpage" => new DeleteIqcTestPageConfig(),
                "deleteiqctestprofilepage" => new DeleteIqcTestProfilePageConfig(),
                "editiqctestprofileantibioticspage" => new EditIqcTestProfileAntibioticsPageConfig(),
                "editiqctestprofiledefaultpageconfig" => new EditIqcTestProfileDefaultPageConfig(),
                "editiqctestqcorganismspage" => new EditIqcTestQcOrganismsPageConfig(),
                "editiqcresultpage" => new EditIqcResultPageConfig(),
                "markiqctestcompletepage" => new MarkIqcTestCompletePageConfig(),
                "runiqctestpage" => new RunIqcTestPageConfig(),
                "selectiqctestprofilepage" => new SelectIqcTestProfilePageConfig(),
                "selectqcorganismspage" => new SelectQcOrganismsPageConfig(),
                _ => null,
            }; ;
        }
    }
}
