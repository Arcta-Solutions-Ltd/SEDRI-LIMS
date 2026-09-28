using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteFormGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteformgroupuievent',
                        description: 'Delete form group',
                        type: 'form',
                        action: 'deleteformgroupform'
                    }";

            return newEvent;
        }
    }
}
