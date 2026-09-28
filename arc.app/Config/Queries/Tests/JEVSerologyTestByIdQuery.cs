using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class JEVSerologyTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'JEVSerologyTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'jevserologytestquerymapper',
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
