using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ASTEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "updateast" => new ASTUpdateEventConfig(),
                _ => null,
            };
        }
    }
}
