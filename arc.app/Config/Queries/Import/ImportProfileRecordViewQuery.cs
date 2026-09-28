//using arc.app.Common;

//namespace arc.app.Config.Queries.Import
//{
//    internal class ImportProfileRecordViewQuery : IDefinition
//    {
//        public string Get()
//        {
//            return @"{ 
//                        'Query': 'importprofilerecordview',
//                        'TableName': 'ImportProfileRecord',
//                        'Type': 'Select',
//                        'Translate': true,
//                        'Fields': [
//                            {'Name': 'Id', 'Type': 'string' },
//                            {'Name': 'ImportProfileId', 'Type': 'string' },
//                            {'Name': 'ColumnNumber', 'Type':'string'},
//                            {'Name': 'FieldName', 'Type':'string'},
//                            {'Name': 'Code', 'Type':'string'},
//                            {'Name': 'LastModifiedDate', 'Type':'datetime'}
//                        ],
//                        'Where' : [
//                            {'Field': 'ImportProfileId', 'Comparison': '=' }
//                        ],
//                        'Orderby': 'ColumnNumber'
//                    }";
//        }
//    }
//}
