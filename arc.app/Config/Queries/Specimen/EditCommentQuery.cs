using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditCommentQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EditCommentQuery',
                        'TableName': 'SpecimenComment',
                        'Type': 'Single',
                        'ResultMapping': 'editcommentquerymapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'SpecimenId', 'Type': 'int'},
                            { 'Name': 'CommentTypeId', 'Type': 'int'},
                            { 'Name': 'CannedCommentId', 'Type': 'int'},
                            { 'Name': 'Comment', 'Type': 'string'},                            
                            { 'Name': 'DisplayOnReport', 'Type': 'string'}
                        ],
                        'Joins': [
                            { 'Table': 'ListItem', 'Type': 'Left', 'On': 'CommentTypeId', 'From': 'Id', 'Fields': [{'Name': 'Value'}] },
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
