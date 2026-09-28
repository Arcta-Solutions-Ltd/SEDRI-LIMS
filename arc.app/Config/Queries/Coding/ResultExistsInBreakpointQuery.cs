using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ResultExistsInBreakpointQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'resultexistsinbreakpoint', 'TableName': 'ResultLine', 'Type': 'Count',
                        'ParameterMapping': 'resultexistsinbreakpointmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ResultId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
