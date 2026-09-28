using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditReportSectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editReportSection',
                        Description: '@ConEdiN@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
