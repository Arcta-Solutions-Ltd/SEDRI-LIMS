using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MovePatientPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'movepatientpage',
                            pageTitle: '@PatMov@',
                            text: '@PatMovA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AccessionNumber', type: 'text', label: '@SpeAcc@'},
                                                { id: 'PatientRef', type: 'text', label: '@PatPatM@'}
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenMer@' }
                        }";

            return page;
        }
    }
}
