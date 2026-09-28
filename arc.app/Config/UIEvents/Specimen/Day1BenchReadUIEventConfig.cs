using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class Day1BenchReadUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'day1benchread',
                        description: 'Bench read for specimens in the culturing state',
                        type: 'form',
                        action: 'day1benchreadform'
                    }";

            return newEvent;
        }
    }
}
