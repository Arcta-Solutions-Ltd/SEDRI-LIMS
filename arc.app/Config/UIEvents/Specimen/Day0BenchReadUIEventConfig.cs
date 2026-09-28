using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class Day0BenchReadUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'day0benchread',
                        description: 'Bench read for specimens in the received state',
                        type: 'form',
                        action: 'day0benchreadform'
                    }";

            return newEvent;
        }
    }
}
