using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RemoveDirectTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'removedirecttestuievent',
                        description: 'Remove Direct Test',
                        type: 'form',
                        action: 'removedirecttestform'
                    }";

            return newEvent;
        }
    }
}
