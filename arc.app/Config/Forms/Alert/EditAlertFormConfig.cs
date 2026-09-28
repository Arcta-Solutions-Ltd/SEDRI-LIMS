using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditAlertFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editalertform',
                        viewTitle: 'Edit an existing alert.',
                        saveEvent: 'editalert',
                        initialQuery: 'editalert',
                        startstate: 'noorganism',
                        suppressRecordView: true,
                        pages: ['editalertdetailspage', 'changeorganismselectorpage', 'editorganismscopepage', 'selectorganismpage','organismlistpage', 'alertdetailssecondpage'],
                        rules:
                        [
                            { Outcome: 'visible', page: 'editorganismscopepage', state: 'neworganism' },
                            { Outcome: 'visible', page: 'selectorganismpage', state: 'organismsearch' },
                            { Outcome: 'visible', page: 'organismlistpage', state: 'organismsearch' }
                        ]
                    }";

            return form;
        }
    }
}
