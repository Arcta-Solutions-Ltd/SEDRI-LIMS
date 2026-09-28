using arc.app.Common;

namespace arc.app.Config.Events
{
    public class MonitoringEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "vieweventdetails" => new ViewEventDetailsEventConfig(),
                _ => null,
            };
        }
    }
}
