using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Query definition for the Export History record view.
/// </summary>
internal class ExportHistoryRecordViewQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'ExportHistoryRecordView',
            'TableName': 'ExportRunHistory',
            'Type': 'special',
            'Translate': true,
            'ResultMapping': 'exporthistoryviewmapper'
        }";
    }
}
