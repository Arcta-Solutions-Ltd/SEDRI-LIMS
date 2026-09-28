using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class LanguageQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "languageexists" => new LanguageExistsQuery(),
                "languagelist" => new LanguageListQuery(),
                "languageusedinlaboratory" => new LanguageUsedInLaboratoryQuery(),
                "languageusedinorganisation" => new LanguageUsedInOrganisationQuery(),
                _ => null,
            };
        }
    }
}
