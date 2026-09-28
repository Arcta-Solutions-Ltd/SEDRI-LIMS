using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class GramCultureTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'GramCultureTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'gramculturetestquerymapper',
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
