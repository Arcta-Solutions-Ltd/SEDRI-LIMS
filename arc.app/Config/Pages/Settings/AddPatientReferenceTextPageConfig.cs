using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddPatientReferenceTextPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'addpatientreferencetextpage',
                            pageTitle: '@SetAddAccTex@',
                            text: '@SetAddPatTexB@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'textvalue', type: 'singleline', label: '@GenTex@', required: true, placeholder: '@SetAddPatTexC@'  }
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
