using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteLocationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletelocationuievent',
                        description: 'Delete Location',
                        type: 'form',
                        action: 'deletelocationform'
                    }";

            return newEvent;
        }
    }
}
