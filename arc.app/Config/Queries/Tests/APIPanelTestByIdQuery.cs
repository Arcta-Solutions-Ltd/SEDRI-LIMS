using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class APIPanelTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'ApiPanelTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'apipaneltestquerymapper',
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
