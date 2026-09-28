using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrderTableUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'ordertableuievent',
                        description: 'Order Table',
                        type: 'form',
                        action: 'ordertableform'
                    }";

            return newEvent;
        }
    }
}
