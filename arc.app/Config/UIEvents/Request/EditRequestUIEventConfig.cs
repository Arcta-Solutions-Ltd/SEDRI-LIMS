using arc.app.Common;

namespace arc.app.Config.UIEvents.Request;

internal class EditRequestUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editrequestuievent',
                        description: 'Edit request',
                        type: 'form',
                        action: 'editrequestform'
                    }";
    }
}
