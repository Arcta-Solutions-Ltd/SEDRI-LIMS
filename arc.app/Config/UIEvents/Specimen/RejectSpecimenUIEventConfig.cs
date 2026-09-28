using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class RejectSpecimenUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'rejectspecimen',
                        description: 'Reject specimen',
                        type: 'form',
                        action: 'rejectspecimenform'
                    }";

            return newEvent;
        }
    }
}
