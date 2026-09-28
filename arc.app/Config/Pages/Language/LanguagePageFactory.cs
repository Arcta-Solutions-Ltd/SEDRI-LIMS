using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class LanguagePageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlanguagepage" => new AddLanguagePageConfig(),
                "deletelanguagepage" => new DeleteLanguagePageConfig(),
                "editwordpage" => new EditWordPageConfig(),
                _ => null,
            };
        }
    }
}
