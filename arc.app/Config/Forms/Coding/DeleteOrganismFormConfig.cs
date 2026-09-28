using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteOrganismFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteorganismform',
                        viewTitle: 'Delete an organism.',
                        saveEvent: 'deleteorganism',
                        initialQuery: 'customentrybyorganismid',
                        suppressRecordView: true,
                        pages: ['deleteorganismpage']
                    }";

            return form;
        }
    }
}
