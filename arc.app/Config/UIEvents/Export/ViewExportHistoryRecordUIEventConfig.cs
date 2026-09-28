using arc.app.Common;

namespace arc.app.Config.UIEvents.Export;

/// <summary>
/// UI event configuration for viewing an export history record.
/// </summary>
internal class ViewExportHistoryRecordUIEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'viewexporthistoryrecorduievent',
            description: 'View export history record',
            type: 'view-record',
            action: 'exporthistory'
        }";
    }
}
