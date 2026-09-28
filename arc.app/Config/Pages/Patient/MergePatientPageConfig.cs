using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MergePatientPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'mergepatientpage',
                            pageTitle: '@PatMer@',
                            text: '@PatMerB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'text', label: '@PatPatL@'},
                                                { id: 'NewPatientRef', type: 'text', label: '@PatPatM@'}
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
