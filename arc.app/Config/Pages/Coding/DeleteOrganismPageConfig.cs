using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteOrganismPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteorganismpage',
                            pageTitle: '@CodDelB@',
                            text: '@CodDelC@.',
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
                                                { id: 'Description', type: 'text', label: '@GenOrgA@' },
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
