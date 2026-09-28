using arc.app.Common;

namespace arc.app.Config.UIEvents;

internal class EditCultureTestOverrideUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'editculturetestoverrideuievent',
            description: 'Edit Culture Test TAT Override',
            type: 'form',
            action: 'editculturetestoverrideform'
        }";
    }
}
