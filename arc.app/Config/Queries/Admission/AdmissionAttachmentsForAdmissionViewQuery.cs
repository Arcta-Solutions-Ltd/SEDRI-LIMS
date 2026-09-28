using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Query for admission attachments display on the admission record view.
/// Returns aggregated admission and child request attachment ids.
/// </summary>
internal class AdmissionAttachmentsForAdmissionViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'AdmissionAttachmentsForAdmissionView',
            'TableName': 'Admission',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'admissionattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'admissionallfileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ],
            'Tags': 'SP'
        }";
    }
}
