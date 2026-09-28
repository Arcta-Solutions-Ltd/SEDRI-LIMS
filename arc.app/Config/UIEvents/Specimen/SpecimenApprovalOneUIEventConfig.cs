using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SpecimenApprovalOneUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimenapprovaloneuievent',
                        description: 'Specimen Approval Level One',
                        type: 'form',
                        action: 'specimenapprovaloneform'
                    }";

            return newEvent;
        }
    }
}
