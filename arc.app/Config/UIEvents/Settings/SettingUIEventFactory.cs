using arc.app.Common;
using arc.app.Config.UIEvents.Settings;

namespace arc.app.Config.UIEvents
{
    internal class SettingUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {

            return definitionName.ToLower() switch
            {
                "addaccessionnumbertextuievent" => new AddAccessionNumberTextUIEventConfig(),
                "addpatientreferencetextuievent" => new AddPatientReferenceTextUIEventConfig(),
                "deletesettinguievent" => new DeleteSettingUIEventConfig(),
                "editaccessionnumberuievent" => new EditAccessionNumberUIEventConfig(),
                "editpatientreferenceuievent" => new EditPatientReferenceUIEventConfig(),
                "editsettinguievent" => new EditSettingUIEventConfig(),
                _ => null,
            };
        }
    }
}
