using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteSectionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletesectionuievent',
                        description: 'Delete existing section',
                        type: 'form',
                        action: 'deletesectionform'
                    }";

            return newEvent;
        }
    }
}
