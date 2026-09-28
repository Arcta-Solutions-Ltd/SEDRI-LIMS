using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddMenuOptionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addMenuOption',
                        Description: '@ConAddC@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
