using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ZNStainTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ZNStainTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'znstaintestquerymapper',
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
