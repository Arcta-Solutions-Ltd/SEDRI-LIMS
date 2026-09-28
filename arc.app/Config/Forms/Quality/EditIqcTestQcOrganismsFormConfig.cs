using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditIqcTestQcOrganismsFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestqcorganismsform',
                viewTitle: '@QuaEdiQcOrgFor@.',
                saveEvent: 'editiqctestqcorganisms',
                initialQuery: 'editiqctestqcorganismsquery',
                suppressRecordView: true,
                pages: ['editiqctestqcorganismspage']
            }";
        }
    }
}
