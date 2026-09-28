using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CommentListByPatientIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CommentListByPatientId',
                        'TableName': 'PatientComment',
                        'Type': 'Select',
                        'Fields': [
                            { 'Name': 'Comment', 'Type': 'string'},
                            { 'Name': 'LastModifiedDate', 'Type': 'datetime' }
                        ],
                        'Where' : [
                            {'Field': 'PatientId', 'Comparison': '=' } 
                        ],
                        'Orderby': 'LastModifiedDate',
                        'Descending': true,
                        'Tags': 'PA'
                    }";
        }
    }
}
