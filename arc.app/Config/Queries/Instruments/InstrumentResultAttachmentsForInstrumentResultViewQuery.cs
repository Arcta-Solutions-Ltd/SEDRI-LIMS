using arc.app.Common;

namespace arc.app.Config.Queries.Instruments;

/// <summary>
/// Query for instrument result attachments on the instrument result record view (comma-separated file attachment ids).
/// </summary>
internal class InstrumentResultAttachmentsForInstrumentResultViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'InstrumentResultAttachmentsForInstrumentResultView',
            'TableName': 'InstrumentResults',
            'Type': 'Single',
            'Translate': true,
            'ResultMapping': 'instrumentresultattachmentsviewmapper',
            'Fields': [
                { 'Name': 'Id', 'KnownAs': 'id' },
                { 'Name': 'FileAttachmentIds', 'Type': 'instrumentresultfileattachmentids', 'KnownAs': 'fileattachmentids' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}
