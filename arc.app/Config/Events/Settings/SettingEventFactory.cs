using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class SettingEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addaccessionnumbertext" => new AddAccessionNumberTextEventConfig(),
                "addpatientreferencetext" => new AddPatientReferenceTextEventConfig(),
                "deletesetting" => new DeleteSettingEventConfig(),
                "editaccessionnumber" => new EditAccessionNumberEventConfig(),
                "editpatientreference" => new EditPatientReferenceEventConfig(),
                "editsetting" => new EditSettingEventConfig(),
                _ => null,
            };
        }
    }
}
