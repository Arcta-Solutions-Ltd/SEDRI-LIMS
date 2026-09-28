using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditOrganisationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editorganisationform',
                        Title: 'Edit an existing organisation.',
                        saveEvent: 'editorganisation',
                        initialQuery: 'organisationbyid',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['organisationeditpage']
                    }";

            return form;
        }
    }
}
