using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteOrganismUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                            name: 'deleteorganismuievent',
                            description: 'Delete Organism',
                            type: 'form',
                            action: 'deleteorganismform'
                        }";

            return newEvent;
        }
    }
}
