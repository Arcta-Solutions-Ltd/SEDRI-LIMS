using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddFormGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addformgroupuievent',
                        description: 'Add form group',
                        type: 'form',
                        action: 'addformgroupform'
                    }";

            return newEvent;
        }
    }
}
