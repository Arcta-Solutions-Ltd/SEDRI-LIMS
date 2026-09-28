using arc.app.Common;

namespace arc.app.Config.UIEvents;

internal class EditDirectTestOverrideUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'editdirecttestoverrideuievent',
            description: 'Edit Direct Test TAT Override',
            type: 'form',
            action: 'editdirecttestoverrideform'
        }";
    }
}
