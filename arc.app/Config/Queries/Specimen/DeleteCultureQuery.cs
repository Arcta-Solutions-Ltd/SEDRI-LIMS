//using arc.app.Common;
//using arc.domain.Coding;

//namespace arc.app.Config.Queries;

//internal class DeleteCultureQuery : IDefinition
//{
//    public string Get()
//    {
//        return @"{ 
//                        'Query': 'DeleteCultureQuery',
//                        'TableName': 'Culture',
//                        'Type': 'Single',
//                        'Fields': [
//                            { 'Name': 'Id', 'Type': 'int'}
//                        ],
//                        'Joins': [
//                            { 'Table': 'ListItem', 'On': 'TypeId', 'From':'Id', 'Fields': [{'Name': 'Value'}] }
//                        ],
//                        'Where' : [
//                            {'Field': 'Id', 'Comparison': '=' } 
//                        ]
//                    }";
//    }
//}

