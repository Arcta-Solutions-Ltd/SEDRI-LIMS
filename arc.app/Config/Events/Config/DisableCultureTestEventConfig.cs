using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DisableCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'disableCultureTest',
                        Description: '@ConDis@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
