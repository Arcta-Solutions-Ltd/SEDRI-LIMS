using arc.app.Common;
    
namespace arc.app.Config.UIEvents.Settings;
internal class AddPatientReferenceTextUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'addpatientreferencetextuievent',
                        description: 'Add Patient Reference Text',
                        type: 'form',
                        action: 'addpatientreferencetextform'
                    }";

        return newEvent;
    }
}
