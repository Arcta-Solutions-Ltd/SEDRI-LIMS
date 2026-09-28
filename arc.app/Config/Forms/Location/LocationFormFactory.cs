using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class LocationFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlocationform" => new AddLocationFormConfig(),
                "deletelocationform" => new DeleteLocationFormConfig(),
                "editlocationform" => new EditLocationFormConfig(),
                _ => null,
            };
        }
    }
}
