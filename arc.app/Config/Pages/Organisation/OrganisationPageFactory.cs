using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrganisationPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "deleteorganisationpage" => new DeleteOrganisationPageConfig(),
                "organisationdetailspage" => new OrganisationDetailsPageConfig(),
                "organisationeditpage" => new OrganisationEditPageConfig(),
                _ => null,
            };
        }
    }
}
