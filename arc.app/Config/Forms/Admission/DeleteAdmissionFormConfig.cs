using arc.app.Common;

namespace arc.app.Config.Forms.Admission;

internal class DeleteAdmissionFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
                        name: 'deleteadmissionform',
                        viewTitle: '@NeoAdmDel@',
                        saveEvent: 'deleteadmission',
                        initialQuery: 'editadmissionquery',
                        suppressRecordView: true,
                        pages: [ 'deleteadmissionpage' ]
                    }";
    }
}
