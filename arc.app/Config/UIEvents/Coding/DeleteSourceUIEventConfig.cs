using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteSourceUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletesourceuievent',
                        description: 'Delete Source',
                        type: 'form',
                        action: 'deletesourceform'
                    }";

            return newEvent;
        }
    }
}
