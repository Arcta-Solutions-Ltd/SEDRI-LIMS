using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class LanguageEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {

            return definitionName.ToLower() switch
            {
                "addlanguage" => new AddLanguageEventConfig(),
                "deletelanguage" => new DeleteLanguageEventConfig(),
                "editword" => new EditWordEventConfig(),
                _ => null,
            };
        }
    }
}
