using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding
{
    internal class DeleteAntibioticGroupUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'deleteantibioticgroupuievent',
                        description: 'Delete Antibiotic Group',
                        type: 'form',
                        action: 'deleteantibioticgroupform'
                    }";

            return newEvent;
        }
    }
}
