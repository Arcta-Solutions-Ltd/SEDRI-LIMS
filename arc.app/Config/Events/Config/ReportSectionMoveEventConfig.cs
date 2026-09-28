using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ReportSectionMoveEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'reportSectionMove',
                        Description: 'Move Report Section',
                        EventType : 'special',
                        Topic : 'Configuration'
                    }";
        }
    }
}
