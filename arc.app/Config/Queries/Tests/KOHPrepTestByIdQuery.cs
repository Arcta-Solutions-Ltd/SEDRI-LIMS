using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class KOHPrepTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'KOHPrepTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'kohpreptestquerymapper',
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
