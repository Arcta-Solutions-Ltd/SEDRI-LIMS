using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class BetalactamaseTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'BetalactamaseTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'betalactamasetestquerymapper',
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
