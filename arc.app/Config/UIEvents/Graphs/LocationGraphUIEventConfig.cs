using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class LocationGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'locationgraphuievent',
                        description: 'Location Graph',
                        type: 'graph',
                        action: 'locationgraph'
                    }";

            return newEvent;
        }
    }
}
