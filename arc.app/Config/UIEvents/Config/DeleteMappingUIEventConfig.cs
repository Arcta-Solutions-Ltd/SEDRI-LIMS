using arc.app.Common;

namespace arc.app.Config.UIEvents.Config;
internal class DeleteMappingUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'deletemappinguievent',
                        description: 'Delete Mapping',
                        type: 'form',
                        action: 'deletemappingform'
                    }";

        return newEvent;
    }
}
