using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class LocationUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlocationuievent" => new AddLocationUIEventConfig(),
                "deletelocationuievent" => new DeleteLocationUIEventConfig(),
                "editlocationuievent" => new EditLocationUIEventConfig(),
                _ => null,
            };
        }
    }
}
