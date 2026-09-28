using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditMenuOptionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editMenuOption',
                        Description: '@ConEdiC@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
