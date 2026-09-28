using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SynonymPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'synonympage',
                            pageTitle: '@OrgManC@',
                            text: '@OrgManE@.',
                            extrawide: false,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'preferredname', type: 'singleline', label: '@OrgPre@' },
                                                { id: 'synonymgrid', type: 'fieldgrid', label: '@OrgSys@', gridfields: [
                                                        { id: 'Synonym', type: 'singleline' }
                                                    ]
                                                }
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
