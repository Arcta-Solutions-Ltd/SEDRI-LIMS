using arc.app.Common;

namespace arc.app.Config.Forms.Admission;

internal class EditAdmissionFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'editadmissionform',
                        viewTitle: '@NeoAdmEdi@',
                        saveEvent: 'editadmission',
                        initialQuery: 'editadmissionquery',
                        suppressRecordView: true,
                        pages: [ 'neoshieldadmissionpage' ]
                    }";
    }
}
