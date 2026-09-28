using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddOrganismUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addorganismuievent',
                        description: 'Add Organism',
                        type: 'form',
                        action: 'addorganismform'
                    }";

            return newEvent;
        }
    }
}
