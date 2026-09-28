using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddSectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addsectionuievent',
                        description: 'Add new section',
                        type: 'form',
                        action: 'addsectionform'
                    }";

            return newEvent;
        }
    }
}
