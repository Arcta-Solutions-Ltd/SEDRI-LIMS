using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteBreakpointEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteBreakpoint', 
                        Description: '@BreDel@',
                        EventType : 'special', 
                        Topic : 'Breakpoints'
                    }";
        }
    }
}
