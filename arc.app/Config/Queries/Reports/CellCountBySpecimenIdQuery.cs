using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CellCountBySpecimenIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'CellCountBySpecimenId', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'cellcounttestquerymapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string'},
                            {'Name': 'SpecimenId', 'Type': 'string'},
                            {'Name': 'TestName', 'Type': 'string'},
                            {'Name': 'TestResults', 'Type': 'string'},
                            {'Name': 'Status', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=', 'FieldToMatch': 'SpecimenId' } 
                        ]
                    }";
        }
    }
}
