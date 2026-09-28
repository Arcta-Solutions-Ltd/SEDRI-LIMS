using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding
{
    internal class AddAntibioticEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addantibioticentryuievent',
                        description: 'Add Antibiotic Entry',
                        type: 'form',
                        action: 'addantibioticentryform'
                    }";

            return newEvent;
        }
    }
}
