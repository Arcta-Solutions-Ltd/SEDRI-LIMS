using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditFormUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editformuievent',
                        description: 'Edit form',
                        type: 'form',
                        action: 'editformform'
                    }";

            return newEvent;
        }
    }
}
