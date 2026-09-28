using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class GramStainTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'GramStainTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'gramstaintestquerymapper',
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
