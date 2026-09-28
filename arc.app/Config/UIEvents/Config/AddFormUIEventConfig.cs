using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddFormUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addformuievent',
                        description: 'Add form',
                        type: 'form',
                        action: 'addformform'
                    }";

            return newEvent;
        }
    }
}
