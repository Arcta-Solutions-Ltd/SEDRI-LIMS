using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddSectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addSection',
                        Description: '@ConAddN@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
