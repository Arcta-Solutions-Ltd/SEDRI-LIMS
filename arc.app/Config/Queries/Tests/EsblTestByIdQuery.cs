using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EsblTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EsblTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'esbltestquerymapper',
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
