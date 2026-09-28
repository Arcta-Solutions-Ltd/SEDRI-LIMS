using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditIqcTestProfileQcOrganismUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestprofileqcorganismuievent',
                description: '@QuaEdiIqcTestProOrg@',
                type: 'form',
                action: 'editiqctestprofileqcorganismform'
            }";
        }
    }
}
