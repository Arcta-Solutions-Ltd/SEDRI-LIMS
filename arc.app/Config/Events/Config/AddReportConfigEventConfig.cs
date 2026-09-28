using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddReportConfigEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addReportConfig',
                        Description: '@ConAddL@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration'
                    }";
        }
    }
}
