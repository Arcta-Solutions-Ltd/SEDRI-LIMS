using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class BatchSpecimenApprovalTwoUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'batchspecimenapprovaltwouievent',
                        description: 'Batch specimen Approval Level Two',
                        type: 'form',
                        action: 'batchspecimenapprovaltwoform'
                    }";

            return newEvent;
        }
    }
}
