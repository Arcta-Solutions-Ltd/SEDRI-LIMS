using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MonitoringJsonViewerUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'monitoringjsonviewer',
                        description: 'View the details of an event',
                        type: 'form',
                        action: 'monitoringjsonviewer'
                    }";

            return newEvent;
        }
    }
}
