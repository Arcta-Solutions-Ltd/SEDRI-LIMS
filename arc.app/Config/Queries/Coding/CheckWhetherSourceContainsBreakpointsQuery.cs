using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CheckWhetherSourceContainsBreakpointsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'checkwhethersourcecontainsbreakpoints', 'TableName': 'Breakpoint', 'Type': 'Special',
                        'ParameterMapping': 'idtosourceidmapper'
                    }";
        }
    }
}
