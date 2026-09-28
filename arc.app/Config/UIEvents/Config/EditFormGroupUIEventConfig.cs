using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditFormGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editformgroupuievent',
                        description: 'Edit form group',
                        type: 'form',
                        action: 'editformgroupform'
                    }";

            return newEvent;
        }
    }
}
