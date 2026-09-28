using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeletePageEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deletePage',
                        Description: '@ConDelX@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
