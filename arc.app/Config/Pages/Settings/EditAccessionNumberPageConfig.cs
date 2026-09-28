using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditAccessionNumberPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editaccessionnumberpage',
                            pageTitle: '@SetOrd@',
                            text: '@SetSet@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'FieldList', type: 'fieldselector', label: '@GenSec@', CanMoveEntries: true, IncludeOptions: false, CanEnable: false}
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
