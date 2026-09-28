using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ImportConfigurationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'importconfigurationuievent',
                        description: 'Import Configuration',
                        type: 'form',
                        action: 'importconfigurationform'
                    }";

            return newEvent;
        }
    }
}
