using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ExportConfigurationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'exportconfiguration',
                        Description: '@ConExpA@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        ValidationRules: [
                        ]
                    }";
        }
    }
}
