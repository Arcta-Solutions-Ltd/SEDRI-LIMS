using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ViewEventDetailsEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'vieweventdetails', 
                        Description: '@MonVie@',
                        Topic : 'Monitoring',
                        EventType : 'special'
                    }";
        }

    }
}
