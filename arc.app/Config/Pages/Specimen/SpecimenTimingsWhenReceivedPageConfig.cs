using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenTimingsWhenReceivedPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'specimentimingswhenreceived',
                            pageTitle: '@SpeSpeM@',
                            text: '@SpeProJ@.',
                            required: 'CollectionDate',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'CollectionDate', type: 'date', label: '@SpeColC@', required: true, placeholder: '@SpeSelI@', Min: 'now d-300', Max: 'now'},
                                                { id: 'CollectionTime', type: 'time', label: '@SpeColD@', placeholder: '@SpeEntI@', mask: '99:99'},
                                                { id: 'ReceivedDate', type: 'date', label: '@SpeRecD@', required: true, defaultToNow: true, placeholder: '@SpeSel@', Min: 'now d-300', Max: 'now'},
                                                { id: 'ReceivedTime', type: 'time', label: '@SpeRecE@', required: true, defaultToNow: true, placeholder: '@SpeEntA@', mask: '99:99'}
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
