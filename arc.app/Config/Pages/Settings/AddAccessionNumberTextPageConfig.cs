using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddAccessionNumberTextPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'addaccessionnumbertextpage',
                            pageTitle: '@SetAddAccTex@',
                            text: '@SetAddAccTexB@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'textvalue', type: 'singleline', label: '@GenTex@', required: true, placeholder: '@SetAddAccTexC@'  }
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
