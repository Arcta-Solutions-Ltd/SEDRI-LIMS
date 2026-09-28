using arc.app.Common;

namespace arc.app.Config.Pages.Coding
{
    internal class DeleteAntibioticPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteantibioticpage',
                            pageTitle: '@AntManDel@',
                            text: '@AntDelSub@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticName', type: 'text', label: '@GenNam@'},
                                                { id: 'Code', type: 'text', label: '@GenCod@'},
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
