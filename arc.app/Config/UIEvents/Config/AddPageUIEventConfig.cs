using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddPageUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addpageuievent',
                        description: 'Add page definition',
                        type: 'form',
                        action: 'addpageform'
                    }";

            return newEvent;
        }
    }
}
