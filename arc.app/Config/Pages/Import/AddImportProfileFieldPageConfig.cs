//using arc.app.Common;

//namespace arc.app.Config.Pages.Import
//{
//    internal class AddImportProfileFieldPageConfig : IDefinition
//    {
//        public string Get()
//        {
//            var page = @"{ 
//                            name: 'addimportprofilefieldpage',
//                            pageTitle: '@ImpAddC@',
//                            text: '@ImpSel@.',
//                            required: 'Name,Heading',
//                            requiredRule: 'and',
//                            columns: [
//                                { 
//                                    key: 'col1',
//                                    fieldWidth: 'wide',
//                                    itemWidth: 'wide',
//                                    formGroups: [
//                                        {
//                                            key: 'fg1',
//                                            fields: [
//                                                { id: 'columnnumber', type: 'number', label: '@ImpCol@', required: true },
//                                            ]
//                                        },
//                                        {
//                                            key: 'fg2',
//                                            rules:[{ effect: 'visible', field: 'TableNameId', rule: '=', value: '9'}],
//                                            fields: [
//                                                { id: 'FieldName', type: 'combobox', label: '@GenFieB@', required: true, multiselect: false, optionsName: 'PatientFieldList' }
//                                            ]
//                                        },
//                                        {
//                                            key: 'fg3',
//                                            rules:[{ effect: 'visible', field: 'TableNameId', rule: '=', value: '10'}],
//                                            fields: [
//                                                { id: 'FieldName', type: 'combobox', label: '@GenFieB@', required: true, multiselect: false, optionsName: 'SpecimenFieldList' }
//                                            ]
//                                        },
//                                        {
//                                            key: 'fg4',
//                                            rules:[{ effect: 'visible', field: 'TableNameId', rule: '=', value: '11'}],
//                                            fields: [
//                                                { id: 'FieldName', type: 'combobox', label: '@GenFieB@', required: true, multiselect: false, optionsName: 'CultureFieldList' }
//                                            ]
//                                        },
//                                        {
//                                            key: 'fg5',
//                                            fields: [
//                                                { id: 'code', type: 'toggle', label: '@GenCodA@', required: false, defaultvalue: '@GenNo@'}
//                                            ]
//                                        }
//                                    ]
//                                }
//                            ]
//                        }";

//            return page;
//        }
//    }
//}
