using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteOrganisationFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'deleteorganisationform',
                    title: 'Delete an organisation',
                    saveEvent: 'deleteorganisation',
                    initialQuery: 'organisationbyid',
                    suppressRecordView: true,
                    configurable: 'Yes',
                    pages: [ 'deleteorganisationpage']
                }";

            return form;
        }
    }
}
