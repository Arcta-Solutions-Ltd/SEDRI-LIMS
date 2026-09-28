using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CellCountTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CellCountTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'cellcounttestquerymapper',
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
