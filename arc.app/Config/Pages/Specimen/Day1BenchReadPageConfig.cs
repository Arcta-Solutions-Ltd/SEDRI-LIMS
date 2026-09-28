using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class Day1BenchReadPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'day1benchread',
                            pageTitle: '@SpeDay@',
                            text: '@SpeAssA@.',
                            nextItemButton: { buttontext: '@SpeNex@', show: true },
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'BenchReadDay1Action', type: 'radio', label: '@GenAct@', required: false, optionsName: 'BenchReadDay1Action', defaultValue: '593' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
