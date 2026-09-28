using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditFormEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editForm',
                        Description: '@ConEdi@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
