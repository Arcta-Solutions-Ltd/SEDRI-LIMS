using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class SettingFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addaccessionnumbertextform" => new AddAccessionNumberTextFormConfig(),
                "addpatientreferencetextform" => new AddPatientReferenceTextFormConfig(),
                "deletesettingform" => new DeleteSettingFormConfig(),
                "editaccessionnumberform" => new EditAccessionNumberFormConfig(),
                "editpatientreferenceform" => new EditPatientReferenceFormConfig(),
                "editsettingform" => new EditSettingFormConfig(),
                _ => null,
            };
        }
    }
}
