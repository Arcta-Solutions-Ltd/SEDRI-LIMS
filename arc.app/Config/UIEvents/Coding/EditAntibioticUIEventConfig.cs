using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditAntibioticUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editantibioticuievent',
                        description: 'Edit Antibiotic',
                        type: 'form',
                        action: 'editantibioticform'
                    }";

            return newEvent;
        }
    }
}
