using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SettingPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addaccessionnumbertextpage" => new AddAccessionNumberTextPageConfig(),
                "addpatientreferencetextpage" => new AddPatientReferenceTextPageConfig(),
                "deletesettingpage" => new DeleteSettingPageConfig(),
                "editaccessionnumberpage" => new EditAccessionNumberPageConfig(),
                "editpatientreferencepage" => new EditPatientReferencePageConfig(),
                "editsettingpage" => new EditSettingPageConfig(),
                _ => null,
            };
        }
    }
}
