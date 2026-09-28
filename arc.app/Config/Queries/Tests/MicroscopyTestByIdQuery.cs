using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class MicroscopyTestByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'MicroscopyTestById', 'TableName': 'Tests', 'Type': 'Single', 'ResultMapping': 'microscopytestquerymapper',
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
