using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RunIqcTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                EventName: 'runiqctest', 
                Description: '@QuaEntA@',
                EventType : 'special', 
                Topic : 'Quality', 
                TableName: 'iqcresults'
            }";
        }
    }
}
