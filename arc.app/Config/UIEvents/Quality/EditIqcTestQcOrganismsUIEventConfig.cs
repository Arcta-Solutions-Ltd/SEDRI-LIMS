using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditIqcTestQcOrganismsUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestqcorganismsuievent',
                description: 'Edit IQC Test QC Organisms UI Event',
                type: 'form',
                action: 'editiqctestqcorganismsform'
            }";
        }
    }
}
