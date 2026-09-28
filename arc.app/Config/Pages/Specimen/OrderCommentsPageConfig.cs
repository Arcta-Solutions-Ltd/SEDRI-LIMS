using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrderCommentsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'ordercommentspage',
                            pageTitle: '@ConPagA@',
                            text: '@ConCha@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@GenComE@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false}
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
