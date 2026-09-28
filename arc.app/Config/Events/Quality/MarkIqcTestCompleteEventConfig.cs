using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class MarkIqcTestCompleteEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'markiqctestcomplete', 
                Description: '@QuaMarIqcTesCom@',
                EventType : 'special', 
                Topic : 'Quality', 
                TableName: 'iqctests'
            }";
        }
    }
}
