using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddAntibioticUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addantibioticuievent',
                        description: 'Add Antibiotic',
                        type: 'form',
                        action: 'addantibioticform'
                    }";

            return newEvent;
        }
    }
}
