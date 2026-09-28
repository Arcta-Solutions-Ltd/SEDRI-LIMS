using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class OrganisationFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addorganisationform" => new AddOrganisationFormConfig(),
                "deleteorganisationform" => new DeleteOrganisationFormConfig(),
                "editorganisationform" => new EditOrganisationFormConfig(),
                _ => null,
            };
        }
    }
}
