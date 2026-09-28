using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrderTablePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'ordertablepage',
                            pageTitle: '@TabOrd@',
                            text: '@TabCha@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@TabTabB@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false }
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
