using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenActionPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'specimenaction',
                            pageTitle: '@SpeSpeH@',
                            viewTitle: 'Specimen Action',
                            text: '@SpeIndA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Message', type: 'multiline', label: '@GenMes@', required: false, placeholder: '@GenAddB@'},
                                                { id: 'Action', type: 'radio', label: '@SpeImm@', required: true, optionsName: 'immediateAction', defaultValue: '510' }
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
