using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddBreakpointPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addbreakpointpage',
                            pageTitle: '@BreAddJ@',
                            text: '@BreAddK@.',
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
                                                        { id: 'resultid', type: 'dropdown', gridTitle: '@GenBreA@', optionsName: 'testresult', dynamic: true },
                                                        { id: 'startval', type: 'number', Min: '0', Max: '9999', MaxDPs: '3', gridTitle: '@GenStaC@', width: 'small' },
                                                        { id: 'endval', type: 'number', Min: '0', Max: '9999', MaxDPs: '3', gridTitle: '@GenEndA@', width: 'small' }
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


