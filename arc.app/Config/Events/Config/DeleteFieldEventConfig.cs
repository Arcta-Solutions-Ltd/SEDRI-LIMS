using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteFieldEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteField',
                        Description: '@ConDelJ@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
