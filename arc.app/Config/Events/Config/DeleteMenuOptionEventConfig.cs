using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteMenuOptionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteMenuOption',
                        Description: '@ConDelC@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
