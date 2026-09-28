using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditTagUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edittaguievent',
                        description: 'Edit Tag',
                        type: 'form',
                        action: 'edittagform'
                    }";

            return newEvent;
        }
    }
}
