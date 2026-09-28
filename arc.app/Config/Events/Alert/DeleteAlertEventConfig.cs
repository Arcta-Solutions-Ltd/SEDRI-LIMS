using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteAlertEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteAlert', 
                        Description: '@AleDel@',
                        EventType : 'special', 
                        Topic : 'Alert', 
                        TableName: 'Alert'
                    }";
        }
    }
}
