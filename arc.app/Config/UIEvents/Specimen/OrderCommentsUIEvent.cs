using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrderCommentsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'ordercommentsuievent',
                        description: 'Order Comments',
                        type: 'form',
                        action: 'ordercommentsform'
                    }";

            return newEvent;
        }
    }
}
