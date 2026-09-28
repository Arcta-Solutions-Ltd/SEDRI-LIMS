using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteIqctestUIEvent : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqctestuievent',
                description: '@QuaDelIqcTes@',
                type: 'form',
                action: 'deleteiqctestform'
            }";
        }
    }
}
