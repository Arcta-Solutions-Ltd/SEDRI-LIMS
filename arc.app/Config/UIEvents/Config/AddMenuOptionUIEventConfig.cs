using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddMenuOptionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addmenuoptionuievent',
                        description: 'Add menu option',
                        type: 'form',
                        action: 'addmenuoptionform'
                    }";

            return newEvent;
        }
    }
}
