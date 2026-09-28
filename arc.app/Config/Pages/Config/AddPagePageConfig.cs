using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddPagePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'addpagepage',
                            pageTitle: '@ConAddW@',
                            text: '@ConAddY@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true },
                                                { id: 'Description', type: 'singleline', label: '@GenDes@', required: true },
                                                { id: 'ShowTableName', type: 'hidden' }
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules: [{ effect: 'visible', field: 'ShowTableName', rule: '=', value: 'Yes' }],
                                            fields: [
                                                { id: 'TableName', type: 'dropdown', label: '@GenTabA@', required: true, Configurable: 'No', options: [
                                                    { key: 'specimen', text: '@GenSpeC@' },
                                                    { key: 'patient', text: '@PatPatI@' },
                                                    { key: 'admission', text: '@NeoAdm@' },
                                                    { key: 'request', text: '@NeoReq@' }
                                                ]}
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
