//using arc.app.Common;

//namespace arc.app.Config.Pages.Import
//{
//    internal class EditImportProfilePageConfig : IDefinition
//    {
//        public string Get()
//        {
//            var page = @"{ 
//                            name: 'editimportprofilepage',
//                            pageTitle: '@ImpEdiA@',
//                            text: '@ImpEntA@',
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
//                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true},
//                                                { id: 'Description', type: 'multiline', label: '@GenDes@', required: true},
//                                                { id: 'IncludeHeaderRow', type: 'toggle', label: '@ImpInc@', required: true, defaultvalue: 'Yes'},
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
