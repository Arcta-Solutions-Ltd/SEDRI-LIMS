using arc.app.Common;

namespace arc.app.Config.UIEvents;

internal class AddDirectTestOverrideUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'adddirecttestoverrideuievent',
            description: 'Add Direct Test TAT Override',
            type: 'form',
            action: 'adddirecttestoverrideform'
        }";
    }
}
