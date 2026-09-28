using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CarbapenemaseTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CarbapenemaseTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'carbapenemasetestquerymapper',
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
