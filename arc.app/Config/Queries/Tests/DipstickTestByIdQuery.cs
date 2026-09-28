using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DipstickTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'DipstickTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'dipsticktestquerymapper',
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
