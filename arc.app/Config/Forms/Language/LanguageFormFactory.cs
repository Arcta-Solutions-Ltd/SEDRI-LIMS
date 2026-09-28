using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class LanguageFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addlanguageform" => new AddLanguageFormConfig(),
                "deletelanguageform" => new DeleteLanguageFormConfig(),
                "editwordform" => new EditWordFormConfig(),
                _ => null,
            };
        }
    }
}
