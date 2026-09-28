using arc.app.Common;

namespace arc.app.Config.UIEvents;

internal class AddCultureTestOverrideUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'addculturetestoverrideuievent',
            description: 'Add Culture Test TAT Override',
            type: 'form',
            action: 'addculturetestoverrideform'
        }";
    }
}
