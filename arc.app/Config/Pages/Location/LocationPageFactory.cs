using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class LocationPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlocationpage" => new AddLocationPageConfig(),
                "editlocationpage" => new EditLocationPageConfig(),
                "deletelocationpage" => new DeleteLocationPageConfig(),
                _ => null,
            };
        }
    }
}
