using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddAntibioticGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addantibioticgroupuievent',
                        description: 'Add Antibiotic Group',
                        type: 'form',
                        action: 'addantibioticgroupform'
                    }";

            return newEvent;
        }
    }
}
