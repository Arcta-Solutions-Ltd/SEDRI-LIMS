using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PregnancyTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PregnancyTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'pregnancytestquerymapper',
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
