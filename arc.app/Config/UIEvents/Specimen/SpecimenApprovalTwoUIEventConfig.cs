using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class SpecimenApprovalTwoUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'specimenapprovaltwouievent',
                        description: 'Specimen Approval Level Two',
                        type: 'form',
                        action: 'specimenapprovaltwoform'
                    }";

            return newEvent;
        }
    }
}
