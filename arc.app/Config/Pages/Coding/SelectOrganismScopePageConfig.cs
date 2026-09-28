using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SelectOrganismScopePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'selectorganismscopepage',
                            pageTitle: '@GenSelH@',
                            text: '@CodSco@.',
                            required: 'OrderId,OrgGroupCodingId, OnlyGenusId',
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
                                                { id: 'OrderId', type: 'dropdown', label: 'Order' },
                                                { id: 'OnlyGenusId', type: 'dropdown', label: 'Genus' },
                                                { id: 'OrgGroupCodingId', type: 'dropdown', label: 'Organism Group' }
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
