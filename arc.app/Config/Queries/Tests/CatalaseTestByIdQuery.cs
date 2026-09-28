using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CatalaseTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CatalaseTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'catalasetestquerymapper',
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
