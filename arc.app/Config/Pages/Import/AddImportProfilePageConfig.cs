//using arc.app.Common;

//namespace arc.app.Config.Pages.Import
//{
//    internal class AddImportProfilePageConfig : IDefinition
//    {
//        public string Get()
//        {
//            var page = @"{ 
//                            name: 'addimportprofilepage',
//                            pageTitle: '@ImpAdd@',
//                            text: '@ImpEnt@',
//                            required: 'Name,Description',
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
//                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, Max: 50},
//                                                { id: 'Description', type: 'multiline', label: '@GenDes@', required: true, Max: 250},
//                                                { id: 'IncludeHeaderRow', type: 'toggle', label: '@ImpInc@', required: true, defaultvalue: '@GenYes@'},
//                                                { id: 'TableNameId', type: 'dropdown', label: '@ImpTab@', required: true, placeholder: '@ImpTab@', optionsName: 'ImportTables' }
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
