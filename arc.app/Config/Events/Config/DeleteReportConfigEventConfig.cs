using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteReportConfigEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteReportConfig',
                        Description: '@ConDelM@',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
