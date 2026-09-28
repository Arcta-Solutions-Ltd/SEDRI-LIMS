using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeletePatientPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletepatientpage',
                            pageTitle: '@PatDel@',
                            text: '@PatDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientRef', type: 'text', label: '@PatPatB@'}
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
