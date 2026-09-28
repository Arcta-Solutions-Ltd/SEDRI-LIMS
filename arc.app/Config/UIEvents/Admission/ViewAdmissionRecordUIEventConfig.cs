using arc.app.Common;

namespace arc.app.Config.UIEvents.Admission;

internal class ViewAdmissionRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'viewadmissionrecorduievent',
                        description: 'View admission record',
                        type: 'view-record',
                        action: 'admissionrecordview'
                    }";
    }
}
