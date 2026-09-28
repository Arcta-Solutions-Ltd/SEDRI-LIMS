using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class TestMethodExistsInBreakpointQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'testmethodexistsinbreakpoint', 'TableName': 'Breakpoint', 'Type': 'Count',
                        'ParameterMapping': 'testmethodexistsinbreakpointmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'TestMethodId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}
