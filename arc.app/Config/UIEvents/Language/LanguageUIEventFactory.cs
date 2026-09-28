using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class LanguageUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlanguageuievent" => new AddLanguageUIEventConfig(),
                "deletelanguageuievent" => new DeleteLanguageUIEventConfig(),
                "editworduievent" => new EditWordUIEventConfig(),
                _ => null,
            };
        }
    }
}
