using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class OxidaseTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'OxidaseTestById', 'TableName': 'CultureTests', 'Type': 'Single', 'ResultMapping': 'oxidasetestquerymapper',
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
