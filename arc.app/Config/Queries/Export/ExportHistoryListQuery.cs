using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Query definition for the Export History list view.
/// </summary>
internal class ExportHistoryListQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'ExportHistoryList',
            'TableName': 'ExportRunHistory',
            'Type': 'special',
            'Translate': true
        }";
    }
}
