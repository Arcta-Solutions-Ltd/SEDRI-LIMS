using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class HPyloriAntigenTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'HPyloriAntigenTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'hpyloriantigentestquerymapper',
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
