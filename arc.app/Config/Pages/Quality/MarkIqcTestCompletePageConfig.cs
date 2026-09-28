using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class MarkIqcTestCompletePageConfig : IDefinition
    {
        public string Get()
        {
            return @"{  
                name: 'markiqctestcompletepage',
                pageTitle: '@QuaMarIqcTesCom@',
                text: '@QuaMarIqcTesComA@',
                columns: [
                    { 
                        key: 'col1',
                        formGroups: [
                            { 
                                key: 'fg1',
                                fields: [
                                    { id: 'warningheader', type: 'plaintext', markup: 'heading', label: '@GenCau@' },
                                    { id: 'warningbody', type: 'plaintext', markup: 'paragraph', label: '@QuaComMes@' },
                                    { id: 'id', type: 'hidden' }
                                ]
                            }
                        ]
                    }
                ]
            }";
        }
    }
}
