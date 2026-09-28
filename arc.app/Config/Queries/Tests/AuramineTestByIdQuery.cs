using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AuramineTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'AuramineTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'auraminetestquerymapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string'},
                            {'Name': 'TestName', 'Type': 'string'},
                            {'Name': 'TestResults', 'Type': 'string'},
                            {'Name': 'Status', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}
