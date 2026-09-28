using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditBreakpointQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'EditBreakpoint',
                        'TableName': 'Breakpoint',
                        'Type': 'Special'
                    }";
        }
    }
}
