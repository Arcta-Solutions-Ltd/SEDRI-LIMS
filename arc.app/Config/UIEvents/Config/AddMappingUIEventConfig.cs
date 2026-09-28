using arc.app.Common;

namespace arc.app.Config.UIEvents.Config;
internal class AddMappingUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'addmappinguievent',
                        description: 'Add Mapping',
                        type: 'form',
                        action: 'addmappingform'
                    }";

        return newEvent;
    }
}
