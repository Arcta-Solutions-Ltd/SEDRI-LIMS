using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddTableEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addtableentryuievent',
                        description: 'Add Table Entry',
                        type: 'form',
                        action: 'addtableentryform'
                    }";

            return newEvent;
        }
    }
}
