using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class TestGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'testgraphuievent',
                        description: 'Test Graph',
                        type: 'graph',
                        action: 'testgraph'
                    }";

            return newEvent;
        }
    }
}
