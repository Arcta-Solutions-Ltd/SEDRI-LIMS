using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class OrganisationEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addorganisation" => new AddOrganisationEventConfig(),
                "deleteorganisation" => new DeleteOrganisationEventConfig(),
                "editorganisation" => new EditOrganisationEventConfig(),
                _ => null,
            };
        }
    }
}
