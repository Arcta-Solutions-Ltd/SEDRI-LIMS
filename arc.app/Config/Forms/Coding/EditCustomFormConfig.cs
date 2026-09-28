using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditCustomFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editcustomform',
                        viewTitle: 'Edit an existing custom entry.',
                        saveEvent: 'editcustomentry',
                        initialQuery: 'customentrybyorganismid',
                        suppressRecordView: true,
                        pages: ['editcustompage']
                    }";

            return form;
        }
    }
}
