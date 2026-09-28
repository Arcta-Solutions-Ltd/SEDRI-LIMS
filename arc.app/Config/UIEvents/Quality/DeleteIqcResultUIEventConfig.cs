using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteIqcResultUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'deleteiqcresultuievent',
                description: '@QuaDelIqcTesRes@',
                type: 'form',
                action: 'deleteiqcresultform'
            }";
        }
    }
}
