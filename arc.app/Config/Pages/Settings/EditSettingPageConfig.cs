using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditSettingPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{  
                            name: 'editsettingpage',
                            pageTitle: '@EdiSetA@',
                            text: '@EdiThe@',
                            wide: false,
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
		                                { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'setting', type: 'text', label: '@GenSetA@' },
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'Type', rule: '=', value: 'yearlist'}],
                                            fields: [
                                                { id: 'YearSetting', type: 'dropdown', label: '@GenVal@', required: true, multiselect: false, optionsName: 'YearSetting'},
                                            ]
                                        },
                                        {
                                            key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'Type', rule: '=', value: 'number'}],
                                            fields: [
                                                { id: 'value', type: 'number', label: '@GenVal@', DefaultValue: '0' },
                                            ]
                                        },
                                        {
                                            key: 'fg4',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'accessionnumber'},
                                                    { effect: 'visible', field: 'type', rule: '=', value: 'text'}
                                                ],
                                            fields: [
                                                { id: 'textvalue', type: 'singleline', label: '@GenTex@', placeholder: '@SetAddAccTexC@' },
                                            ]
                                        },
                                        {
                                            key: 'fg5',
                                            rules:[{ effect: 'visible', field: 'Type', rule: '=', value: 'mapping'}],
                                            fields: [
                                                { id: 'MappingGrid', type: 'fieldgrid', label: '@GenMap@', RemoveGridDeleteButton: true, RemoveGridAddButton: true, gridfields: [
                                                        { id: 'MapType', type: 'text', width: 'large' },
                                                        { id: 'MapValue', type: 'singleline', width: 'narrow', Max: 7}
                                                    ]
                                                }
                                            ]
                                        },
                                        { 
                                            key: 'fg6',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'accessionnumber'},
                                                    { effect: 'visible', field: 'SettingId', rule: '!=', value: 'sequence'}
                                                ],
                                            fields: [
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, defaultValue: 'Yes' }
                                            ]
                                        },
                                        { 
                                            key: 'fg7',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'accessionnumber'},
                                                    { effect: 'visible', field: 'SettingId', rule: '!=', value: 'sequence'},
                                                    { effect: 'visible', field: 'Setting', rule: '!=', value: 'Text'},
                                                    { effect: 'visible', field: 'Enabled', rule: '=', value: 'Yes'}
                                                ],
                                            fields: [
                                                { id: 'Enabled2', type: 'toggle', label: '@GenResC@', required: true, defaultValue: 'Yes' }
                                            ]
                                        },
                                        {
                                            key: 'fg8',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'patientreference'},
                                                    { effect: 'visible', field: 'type', rule: '=', value: 'text'}
                                                ],
                                            fields: [
                                                { id: 'textvalue', type: 'singleline', label: '@GenTex@', placeholder: '@SetAddPatTexC@' },
                                            ]
                                        },
                                        { 
                                            key: 'fg9',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'patientreference'},
                                                    { effect: 'visible', field: 'SettingId', rule: '!=', value: 'sequence'}
                                                ],
                                            fields: [
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, defaultValue: 'Yes' }
                                            ]
                                        },
                                        { 
                                            key: 'fg10',
                                            rules:[
                                                    { effect: 'visible', field: 'Category', rule: '=', value: 'patientreference'},
                                                    { effect: 'visible', field: 'SettingId', rule: '!=', value: 'sequence'},
                                                    { effect: 'visible', field: 'Setting', rule: '!=', value: 'Text'},
                                                    { effect: 'visible', field: 'Enabled', rule: '=', value: 'Yes'}
                                                ],
                                            fields: [
                                                { id: 'Enabled2', type: 'toggle', label: '@GenResC@', required: true, defaultValue: 'Yes' }
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
