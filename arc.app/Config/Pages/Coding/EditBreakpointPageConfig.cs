using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditBreakpointPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editbreakpointpage',
                            pageTitle: '@BreEdi@',
                            text: '@BreEdiA@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'BreakpointGrid', type: 'fieldgrid', gridfields: [
                                                        { id: 'ResultId', type: 'dropdown', gridTitle: '@GenBreA@', optionsName: 'testresult' },
                                                        { id: 'StartVal', type: 'number', Min: '0', Max: '9999', MaxDPs: '3', gridTitle: '@GenStaC@', width: 'small' },
                                                        { id: 'EndVal', type: 'number', Min: '0', Max: '9999', MaxDPs: '3', gridTitle: '@GenEndA@', width: 'small' }
                                                    ]
                                                }
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
