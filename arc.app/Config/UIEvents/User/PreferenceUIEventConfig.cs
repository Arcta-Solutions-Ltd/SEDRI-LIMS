using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PreferenceUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'preferenceuievent',
                        description: 'Preferences',
                        type: 'form',
                        action: 'preferenceform'
                    }";

            return newEvent;
        }
    }
}
