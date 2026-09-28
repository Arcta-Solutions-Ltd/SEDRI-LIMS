using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditReportConfigEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editReportConfig',
                        Description: '@ConEdiL@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
