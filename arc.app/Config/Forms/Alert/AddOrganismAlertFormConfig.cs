using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddOrganismAlertFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addorganismalertform',
                        viewTitle: 'Add a new alert.',
                        saveEvent: 'addorganismalert',
                        suppressRecordView: true,
                        pages: [ 'alertdetailspage', 'editorganismscopepage', 'selectorganismpage','organismlistpage','alertdetailssecondpage'],
                        rules:
                        [
                            { Outcome: 'visible', page: 'selectorganismpage', state: 'organismsearch' },
                            { Outcome: 'visible', page: 'organismlistpage', state: 'organismsearch' }
                        ]
                    }";

            return form;
        }
    }
}
