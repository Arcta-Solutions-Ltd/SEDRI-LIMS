using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class LocationEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlocation" => new AddLocationEventConfig(),
                "deletelocation" => new DeleteLocationEventConfig(),
                "editlocation" => new EditLocationEventConfig(),
                _ => null,
            };
        }
    }
}
