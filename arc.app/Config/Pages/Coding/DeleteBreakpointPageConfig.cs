using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteBreakpointPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletebreakpointpage',
                            pageTitle: '@BreDel@',
                            text: '@BreDelA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AntibioticName', type: 'text', label: '@GenAnt@'},
                                                { id: 'TestMethod', type: 'text', label: '@BreTes@'},
                                                { id: 'Host', type: 'text', label: '@GenHos@'}
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
