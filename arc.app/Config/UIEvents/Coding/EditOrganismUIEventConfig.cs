using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditOrganismUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editorganismuievent',
                        description: 'Edit Organism',
                        type: 'form',
                        action: 'editorganismform'
                    }";

            return newEvent;
        }
    }
}
