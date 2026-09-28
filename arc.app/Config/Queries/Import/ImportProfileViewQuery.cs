//using arc.app.Common;

//namespace arc.app.Config.Queries.Import
//{
//    internal class ImportProfileViewQuery : IDefinition
//    {
//        public string Get()
//        {
//            return @"{ 
//                        'Query': 'importprofileview',
//                        'TableName': 'ImportProfile',
//                        'Type': 'Single',
//                        'Translate': true,
//                        'ResultMapping': 'importprofileviewmapper',
//                        'Fields': [
//                            {'Name': 'Id', 'Type': 'string' },
//                            {'Name': 'Name', 'Type': 'string' },
//                            {'Name': 'Description', 'Type': 'string' },
//                            {'Name': 'IncludeHeaderRow', 'Type': 'string' },
//                            {'Name': 'LastModifiedDate', 'Type':'datetime'}
//                        ],
//                        'ListItems': 'Tablename',
//                        'Where' : [
//                            {'Field': 'Id', 'Comparison': '=' }
//                        ]
//                    }";
//        }
//    }
//}
