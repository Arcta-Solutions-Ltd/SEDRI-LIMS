using arc.app.Common;

namespace arc.app.Config.UIEvents.Request;

internal class DeleteRequestUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deleterequestuievent',
                        description: 'Delete request',
                        type: 'form',
                        action: 'deleterequestform'
                    }";
    }
}
