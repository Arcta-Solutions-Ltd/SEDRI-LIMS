using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class IndiaInkTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'IndiaInkTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'indiainktestquerymapper',
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
