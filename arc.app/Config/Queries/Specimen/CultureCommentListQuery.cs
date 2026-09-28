using arc.app.Common;

namespace arc.app.Config.Queries.Specimen
{
    internal class CultureCommentListQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'CultureCommentListQuery',                        
                        'TableName': 'SpecimenComment', 
                        'Type': 'Special',
                        'Translate': true,
                        'Tags': 'SP'
                    }
            ";
            //return @"{ 
            //            'Query': 'CultureCommentListQuery',
            //            'TableName': 'SpecimenComment',
            //            'Type': 'Select',
            //            'Fields': [
            //                { 'Name': 'Id', 'Type': 'int'},
            //                { 'Name': 'Comment', 'Type': 'string'},
            //                { 'Name': 'CommentTypeId', 'Type': 'int' },
            //                { 'Name': 'LastModifiedDate', 'Type': 'datetime' },
            //                { 'Name': 'AddedBy', 'Type': 'string'},
            //                { 'Name': 'DisplayOnReport', 'Type': 'string' }
            //            ],
            //            'Joins':[
            //                { 'Table': 'ListItem', 'Type': 'Left', 'On': 'CommentTypeId', 'From': 'Id', 'Fields': [{'Name': 'Value'}] },
            //                { 'Table': 'Culture', 'Type': 'Left', 'On': 'CultureId', 'From': 'Id', 'Fields': [{'Name': 'SpecimenId'}] }
            //                ],
            //            'Where': [
            //                {'Field': 'CultureId', 'Comparison': '=' },
            //                {'Field': 'CommentTypeId', 'Comparison': '=', 'Values': ['1232', '1233'] }
            //            ],
            //            'Orderby': 'LastModifiedDate',
            //            'Descending': true
            //        }";
        }
    }
}
