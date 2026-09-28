using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewEventDetailsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'vieweventdetails',
                        description: 'View the details of an event',
                        type: 'form',
                        action: 'vieweventdetailsjson'
                    }";

            return newEvent;
        }
    }
}
