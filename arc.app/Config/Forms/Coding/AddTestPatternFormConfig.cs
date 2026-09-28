using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddTestPatternFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addtestpatternform',
                        title: '@TesAdd@',
                        viewTitle: 'Add a new test pattern.',
                        saveEvent: 'addtestpattern',
                        startstate: '',
                        suppressRecordView: true,
                        pages: ['addtestpatterngeneralpage', 'editorganismscopepage', 'selectorganismpage', 'organismlistpage', 'addtestpatterndetailspage'],
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
