using arc.app.Common;

namespace arc.app.Config.UIEvents.Settings;
internal class EditPatientReferenceUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'editpatientreferenceuievent',
                        description: 'Edit Patient Reference',
                        type: 'form',
                        action: 'editpatientreferenceform'
                    }";

        return newEvent;
    }
}
