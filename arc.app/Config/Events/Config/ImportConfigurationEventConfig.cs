using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ImportConfigurationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'importconfiguration',
                        Description: '@ConImpA@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        DoNotSaveInQueue: true,
                        ValidationRules: [
                        ]
                    }";
        }
    }
}
