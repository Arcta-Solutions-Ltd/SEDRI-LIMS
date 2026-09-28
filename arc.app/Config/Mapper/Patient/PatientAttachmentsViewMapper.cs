using arc.app.Common;

namespace arc.app.Config.Mapper.Patient;

/// <summary>
/// Mapper for patient attachments region. Maps aggregated patient, admission and request
/// attachment ids to a single attachments section on the patient record view.
/// </summary>
internal class PatientAttachmentsViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'patientattachmentsviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:42:>', Type: 'Mapping', Source: 'fileattachmentids', Value: 'fileattachmentids' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'attachments',
                        Title: '@GenAtts@',
                        Fields: [
                            { Id: 'fileattachmentids', Label: '', Type: 'upload', Value: '<:42:>' }
                        ]
                    }
                ]
            }
        }";
    }
}
