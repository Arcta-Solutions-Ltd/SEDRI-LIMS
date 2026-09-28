using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Query for export history attachments display. Returns Id and FileAttachmentId for the export history record view attachments region.
/// </summary>
internal class ExportHistoryAttachmentsForRecordViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'ExportHistoryAttachmentsForRecordView',
            'TableName': 'ExportRunHistory',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'exporthistoryattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentId', 'KnownAs': 'fileattachmentid' }
            ],
            'Where': [
                {'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
