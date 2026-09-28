using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddOrganisationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addorganisationform',
                        title: 'Add a new organisation.',
                        saveEvent: 'addorganisation',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['organisationdetailspage']
                    }";

            return form;
        }
    }
}
