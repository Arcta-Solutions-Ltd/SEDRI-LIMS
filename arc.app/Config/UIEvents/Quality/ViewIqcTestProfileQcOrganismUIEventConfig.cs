using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewIqcTestProfileQcOrganismUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'viewiqctestprofileqcorganismuievent',
                description: 'View QC Organism record',
                type: 'view-record',
                action: 'iqctestprofile'
            }";
        }
    }
}
