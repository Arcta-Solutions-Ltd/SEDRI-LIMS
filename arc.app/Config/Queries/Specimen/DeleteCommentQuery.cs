using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteCommentQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'DeleteCommentQuery', 
                        'TableName': 'SpecimenComment', 
                        'Type': 'Single',
                        'ResultMapping': 'deletecommentmapper',
                        'Fields': [
                            { 'Name': 'Comment', 'Type': 'string' },                        
                            { 'Name': 'Id', 'Type': 'string' },
                            { 'Name': 'CannedCommentId', 'Type': 'int'}
                        ],
                        'Joins': [
                            { 'Table': 'ListItem', 'Type': 'Left', 'On': 'CannedCommentId', 'From': 'Id', 'Fields': [{'Name': 'Value'}] }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ],
                        'Tags': 'SP'
                    }";
        }
    }
}
