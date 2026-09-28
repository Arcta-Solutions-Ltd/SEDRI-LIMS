using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditBreakpointFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editbreakpointform',
                        viewTitle: 'Edit an existing breakpoint.',
                        title: '@BreAddL@',
                        saveEvent: 'editbreakpoint',
                        initialQuery: 'editbreakpoint',
                        startstate: '',
                        suppressRecordView: true,
                        pages: ['changeorganismselectorpage','editorganismscopepage', 'selectorganismpage','organismlistpage', 'editbreakpointcriteriapage', 'editbreakpointpage'],
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
