using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditMenuOptionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editmenuoptionuievent',
                        description: 'Edit menu option',
                        type: 'form',
                        action: 'editmenuoptionform'
                    }";

            return newEvent;
        }
    }
}
