using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrganisationUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addorganisationuievent" => new AddOrganisationUIEventConfig(),
                "deleteorganisationuievent" => new DeleteOrganisationUIEventConfig(),
                "editorganisationuievent" => new EditOrganisationUIEventConfig(),
                _ => null,
            };
        }
    }
}