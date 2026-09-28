using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditCustomUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editcustomuievent',
                        description: 'Edit Custom Entry',
                        type: 'form',
                        action: 'editcustomform'
                    }";

            return newEvent;
        }
    }
}
