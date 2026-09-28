using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditTestPatternFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edittestpatternform',
                        viewTitle: 'Edit a test pattern.',
                        title: '@TesEdiE@',
                        saveEvent: 'edittestpattern',
                        initialQuery: 'edittestpattern',
                        suppressRecordView: true,
                        startstate: '',
                        pages: ['edittestpatterngeneralpage', 'changeorganismselectorpage', 'editorganismscopepage', 'selectorganismpage','organismlistpage', 'edittestpatterndetailspage'],
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
