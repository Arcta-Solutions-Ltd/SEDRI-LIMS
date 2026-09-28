//using arc.app.Common;

//namespace arc.app.Config.Queries.Import
//{
//    internal class ImportFileListQuery : IDefinition
//    {
//        public string Get()
//        {
//            return @"{ 
//                        'Query': 'ImportFileListQuery',
//                        'TableName': 'ImportFileHistory',
//                        'Type': 'Select',
//                        'Translate': true,
//                        'Fields': [
//                            { 'Name': 'Id', 'Type': 'int'},
//                            { 'Name': 'FileName', 'Type': 'string' },
//                            { 'Name': 'LastModifiedDate', 'Type': 'datetime'}
//                        ],
//                        'Joins': [
//                            { 'Table': 'ImportProfile', 'Fields': [{'Name': 'Name'}] }
//                        ],
//                        'ListItems': 'ImportStatus'
//                    }";
//        }
//    }
//}
