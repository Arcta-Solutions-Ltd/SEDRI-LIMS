using arc.app.Common;

namespace arc.app.Config.UIEvents.Admission;

internal class EditAdmissionUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editadmissionuievent',
                        description: 'Edit admission',
                        type: 'form',
                        action: 'editadmissionform'
                    }";
    }
}
