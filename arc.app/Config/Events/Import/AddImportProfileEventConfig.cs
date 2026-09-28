//using arc.app.Common;

//namespace arc.app.Config.Events.Import
//{
//    internal class AddImportProfileEventConfig : IDefinition
//    {
//        public string Get()
//        {
//            return @"{ 
//                        EventName: 'addimportprofile',
//                        Description: '@ImpAddA@',
//                        EventType : 'specialadddata',
//                        Topic : 'Import',
//                        TableName: 'ImportProfile',
//                        ValidationRules: [
//                            { field: 'Name', rule: 'required', message: '@QuaAddProC@'},
//                            { field: 'Description', rule: 'required', message: '@ConAde@'},
//                            { field: 'TableNameId', rule: 'required', message: '@ImpYou@'}
//                        ],
//                        DataRules: [ 
//                            { type: 'NoRecord', query: 'importprofilealreadyexistsforadd', message: '@ImpAni@' }
//                        ]
//                    }";
//        }
//    }
//}
