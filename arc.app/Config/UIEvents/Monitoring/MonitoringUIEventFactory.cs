using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MonitoringUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "monitoringjsonviewer" => new MonitoringJsonViewerUIEventConfig(),
                "vieweventdetails" => new ViewEventDetailsUIEventConfig(),
                _ => null,
            };
        }
    }
}
