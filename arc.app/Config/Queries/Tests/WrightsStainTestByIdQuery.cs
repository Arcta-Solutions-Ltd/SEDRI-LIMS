using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class WrightsStainTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'WrightsStainTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'wrightsstaintestquerymapper',
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
