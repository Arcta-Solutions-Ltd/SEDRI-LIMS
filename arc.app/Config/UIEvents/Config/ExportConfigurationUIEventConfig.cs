using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ExportConfigurationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'exportconfigurationuievent',
                        description: 'Export Configuration',
                        type: 'form',
                        action: 'exportconfigurationform'
                    }";

            return newEvent;
        }
    }
}
