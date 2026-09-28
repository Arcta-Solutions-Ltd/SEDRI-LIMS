using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BiochemistryTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'BiochemistryTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'biochemistrytestquerymapper',
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
