using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ViewConfigUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'viewconfiguievent',
                        description: 'View ui event config',
                        type: 'view-record',
                        action: 'views'
                    }";

            return newEvent;
        }
    }
}
