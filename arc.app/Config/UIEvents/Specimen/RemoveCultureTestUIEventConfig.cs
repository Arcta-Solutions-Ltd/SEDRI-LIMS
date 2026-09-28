using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RemoveCultureTestUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'removeculturetestuievent',
                        description: 'Delete Culture Test',
                        type: 'form',
                        action: 'removeculturetestform'
                    }";

            return newEvent;
        }
    }
}
