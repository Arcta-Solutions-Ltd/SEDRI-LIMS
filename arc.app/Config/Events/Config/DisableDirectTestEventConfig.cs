using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DisableDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'disableDirectTest',
                        Description: '@ConDisA@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
