using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class LanguageMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "deletelanguagemapper" => new DeleteLanguageMapper(),
                "languageexistsmapper" => new LanguageExistsMapper(),
                "languageusedinlaboratorymapper" => new LanguageUsedInLaboratoryMapper(),
                "languageusedinorganisationmapper" => new LanguageUsedInOrganisationMapper(),
                _ => null,
            };
        }
    }
}
