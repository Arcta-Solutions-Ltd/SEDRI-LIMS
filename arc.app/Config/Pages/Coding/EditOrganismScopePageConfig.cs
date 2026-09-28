using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditOrganismScopePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editorganismscopepage',
                            pageTitle: '@GenSelI@',
                            text: '@CodScoA@.',
                            required: 'OrderId,OrgGroupCodingId',
                            requiredRule: 'or',
                            requiredError: '@ValOr@',
                            crafted: true,
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'OrderId', type: 'dropdown', label: '', requred: true },
                                                { id: 'OrgGroupCodingId', type: 'dropdown', label: 'Organism Group' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { onclickstate: { state: 'organismsearch',
                                                          rules:[{ effect: 'organismsearch', field: 'SourceOfClick', rule: '=', value: 'customnovalidation'}]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: true
                                        }
                        }";

            return page;
        }
    }
}
