using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding
{
    internal class DeleteAntibioticEntryUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'deleteantibioticentryuievent',
                        description: 'Delete Antibiotic Entry',
                        type: 'form',
                        action: 'deleteantibioticentryform'
                    }";

            return newEvent;
        }
    }
}
