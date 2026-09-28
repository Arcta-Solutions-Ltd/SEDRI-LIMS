using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddTableUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addtableuievent',
                        description: 'Add Table',
                        type: 'form',
                        action: 'addtableform'
                    }";

            return newEvent;
        }
    }
}
