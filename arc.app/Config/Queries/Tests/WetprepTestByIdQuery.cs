using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class WetprepTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'WetprepTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'wetpreptestquerymapper',
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
