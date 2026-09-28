using arc.app.Common;

namespace arc.app.Config.UIEvents.Config;
internal class EditMappingUIEventConfig : IDefinition
{
    public string Get()
    {
        var newEvent = @"{
                        name: 'editmappinguievent',
                        description: 'Edit Mapping',
                        type: 'form',
                        action: 'editmappingform'
                    }";

        return newEvent;
    }
}
