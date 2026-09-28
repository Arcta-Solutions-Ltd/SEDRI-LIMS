using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class OrganismSusceptibilityGraphUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'organismsusceptibilitygraphuievent',
                        description: 'Organism Graph',
                        type: 'graph',
                        action: 'organismsusceptibilitygraph'
                    }";

            return newEvent;
        }
    }
}
