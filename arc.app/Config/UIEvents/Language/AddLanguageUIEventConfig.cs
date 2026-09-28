using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddLanguageUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addlanguageuievent',
                        description: 'Add Language',
                        type: 'form',
                        action: 'addlanguageform'
                    }";

            return newEvent;
        }
    }
}
