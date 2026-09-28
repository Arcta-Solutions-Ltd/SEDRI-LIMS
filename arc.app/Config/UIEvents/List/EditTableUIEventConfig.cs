using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditTableUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edittableuievent',
                        description: 'Edit Table',
                        type: 'form',
                        action: 'edittableform'
                    }";

            return newEvent;
        }
    }
}
