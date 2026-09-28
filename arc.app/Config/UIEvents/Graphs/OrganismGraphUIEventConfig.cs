using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrganismGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'organismgraphuievent',
                        description: 'Organism Graph',
                        type: 'graph',
                        action: 'organismgraph'
                    }";

            return newEvent;
        }
    }
}
