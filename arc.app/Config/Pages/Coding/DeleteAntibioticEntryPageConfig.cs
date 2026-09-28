using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class DeleteAntibioticEntryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteantibioticentrypage',
                            pageTitle: '@AntDelB@',
                            text: '@AntDelC@.',
                            required: 'Name',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticName', type: 'text', label: '@RepAnt@' },
                                                { id: 'Code', type: 'text', label: '@GenCodA@' }
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
