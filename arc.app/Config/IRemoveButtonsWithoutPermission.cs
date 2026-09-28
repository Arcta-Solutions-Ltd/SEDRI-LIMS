using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using System.Collections.Generic;

namespace arc.app.Config
{
    public interface IRemoveButtonsWithoutPermission
    {
        List<ListViewConfig> RemoveButtonsFromListView(List<ListViewConfig> listView, List<UIEventConfig> allowedUIEvents);
        List<RecordViewConfig> RemoveButtonsFromRecordView(List<RecordViewConfig> recordView, List<UIEventConfig> allowedUIEvents);
    }
}
