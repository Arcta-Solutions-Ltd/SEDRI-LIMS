using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RejectSpecimenConditionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'rejectspecimencondition',
                        description: 'Reject Specimen Condition',
                        type: 'form',
                        action: 'rejectspecimenconditionform'
                    }";

            return newEvent;
        }
    }
}
