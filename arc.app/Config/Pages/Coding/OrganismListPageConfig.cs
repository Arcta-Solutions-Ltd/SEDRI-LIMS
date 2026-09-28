using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrganismListPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'organismlistpage',
                            pageGroup: 'organism',
                            groupAnchor: 3,
                            pageTitle: '@GenOrgD@',
                            text: '@CodLis@.',
                            queryName: 'organismsearch',
                            configureActions: 'nofields',
                            crafted: true,
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [

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
