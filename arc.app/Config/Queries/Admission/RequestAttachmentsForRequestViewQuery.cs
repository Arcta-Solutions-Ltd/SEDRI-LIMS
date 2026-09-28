using arc.app.Common;

namespace arc.app.Config.Queries.Admission;

/// <summary>
/// Query for request attachments display on the request record view.
/// </summary>
internal class RequestAttachmentsForRequestViewQuery : IDefinition
{
    /// <summary>
    /// Retrieves the query definition.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'RequestAttachmentsForRequestView',
            'TableName': 'Request',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'requestattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'requestfileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ],
            'Tags': 'SP'
        }";
    }
}
