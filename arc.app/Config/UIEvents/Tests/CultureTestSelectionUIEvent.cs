using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CultureTestSelectionUIEvent : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'culturetestselectionuievent',
                        description: 'Culture Test Selection',
                        type: 'form',
                        action: 'culturetestselectionform'
                    }";

            return newEvent;
        }
    }
}
