using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteSectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteSection',
                        Description: '@ConDelO@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
