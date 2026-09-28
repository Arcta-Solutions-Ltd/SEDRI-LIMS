using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditTableEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edittableentryuievent',
                        description: 'Edit Table Entry',
                        type: 'form',
                        action: 'edittableentryform'
                    }";

            return newEvent;
        }
    }
}
