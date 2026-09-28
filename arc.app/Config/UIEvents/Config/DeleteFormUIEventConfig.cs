using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteFormUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleteformuievent',
                        description: 'Delete form',
                        type: 'form',
                        action: 'deleteformform'
                    }";

            return newEvent;
        }
    }
}
