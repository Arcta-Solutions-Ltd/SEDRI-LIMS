using arc.domain.Configuration.UIEventsConfig;
using arc.domain.Configuration.ViewConfig.ListViewConfig;
using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using System.Collections.Generic;

namespace arc.app.Config
{
    /// <summary>
    /// This class provides functionality to remove buttons from list and record views that are not included in the allowed UI events.
    /// </summary>
    public class RemoveButtonsWithoutPermission : IRemoveButtonsWithoutPermission
    {
        /// <summary>
        /// Removes buttons from the list view configurations that are not in the allowed UI events list.
        /// </summary>
        /// <param name="listView">The list of list view configurations to process.</param>
        /// <param name="allowedUIEvents">The list of allowed UI events.</param>
        /// <returns>The modified list of list view configurations with unauthorized buttons removed.</returns>
        public List<ListViewConfig> RemoveButtonsFromListView(List<ListViewConfig> listView, List<UIEventConfig> allowedUIEvents)
        {
            foreach (var view in listView)
            {
                if (view.Name.ToLower() == "specimens")
                {
                    var x = 1;
                }
                view.RemoveButtonsNotInUIEventList(allowedUIEvents);
            }

            return listView;
        }

        /// <summary>
        /// Removes buttons from the record view configurations that are not in the allowed UI events list.
        /// </summary>
        /// <param name="recordView">The list of record view configurations to process.</param>
        /// <param name="allowedUIEvents">The list of allowed UI events.</param>
        /// <returns>The modified list of record view configurations with unauthorized buttons removed.</returns>
        public List<RecordViewConfig> RemoveButtonsFromRecordView(List<RecordViewConfig> recordView, List<UIEventConfig> allowedUIEvents)
        {
            foreach (var view in recordView)
            {
                view.RemoveButtonsNotInUIEventList(allowedUIEvents);
            }

            return recordView;
        }
    }
}

