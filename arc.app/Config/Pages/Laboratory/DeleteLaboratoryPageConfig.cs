using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteLaboratoryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletelaboratorypage',
                            pageTitle: '@LabDelA@',
                            text: '@LabDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'LaboratoryName', type: 'text', label: '@LabLabA@'}
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
