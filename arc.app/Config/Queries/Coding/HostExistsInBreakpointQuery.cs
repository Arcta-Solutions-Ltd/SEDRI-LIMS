using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class HostExistsInBreakpointQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'hostexistsinbreakpoint', 'TableName': 'Breakpoint', 'Type': 'Count',
                        'ParameterMapping': 'hostexistsinbreakpointmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'HostId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
