using arc.app.Common;

namespace arc.app.Config.UIEvents.Admission;

internal class DeleteAdmissionUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deleteadmissionuievent',
                        description: 'Delete admission',
                        type: 'form',
                        action: 'deleteadmissionform'
                    }";
    }
}
