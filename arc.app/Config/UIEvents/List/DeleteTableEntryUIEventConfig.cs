using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteTableEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletetableentryuievent',
                        description: 'Delete Table Entry',
                        type: 'form',
                        action: 'deletetableentryform'
                    }";

            return newEvent;
        }
    }
}
