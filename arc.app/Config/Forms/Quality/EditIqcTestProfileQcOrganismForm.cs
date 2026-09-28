using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditIqcTestProfileQcOrganismForm : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestprofileqcorganismform',
                viewTitle: '@QuaEdiA@.',
                initialQuery: 'editiqctestprofileqcorganismquery',
                saveEvent: 'editiqctestprofileqcorganism',
                suppressRecordView: true,
                pages: ['editiqctestprofileantibioticspage', 'editiqctestprofiledefaultpageconfig']
            }";
        }
    }
}
