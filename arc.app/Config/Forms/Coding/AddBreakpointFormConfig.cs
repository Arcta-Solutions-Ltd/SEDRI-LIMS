using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddBreakpointFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addbreakpointform',
                        title: '@BreAdd@',
                        viewTitle: 'Add a new breakpoint.',
                        saveEvent: 'addbreakpoint',
                        startstate: '',
                        suppressRecordView: true,
                        pages: ['editorganismscopepage', 'selectorganismpage','organismlistpage', 'addbreakpointcriteriapage', 'addbreakpointpage'],
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
