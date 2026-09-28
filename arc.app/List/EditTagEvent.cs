using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.List
{
    /// <summary>
    /// Handles the edittag special event. Updates an existing tag (ListItem with ListId=105)
    /// and refreshes its parent relationship in listitemparentchild.
    /// </summary>
    internal class EditTagEvent : IRun
    {
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditTagEvent"/> class.
        /// </summary>
        /// <param name="listRepository">The list repository for persisting tag changes.</param>
        /// <param name="logWriter">The log writer for debugging.</param>
        public EditTagEvent(IListRepository listRepository, ILogWriter logWriter)
        {
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Runs the edit tag event. Deserializes the mapped data, builds a ListItemModel,
        /// and persists via EditTableEntryAsync (which updates listitemparentchild).
        /// </summary>
        /// <param name="dataToSave">JSON with Id, Value (tag name), ParentTagId, Enabled.</param>
        /// <param name="id">The tag Id being edited (from record context).</param>
        /// <param name="command">The event command.</param>
        /// <param name="eventData">The event configuration.</param>
        /// <returns>The edited tag Id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sourceData = JsonConvert.DeserializeObject<EditTagMappedModel>(dataToSave);
            var tagId = sourceData?.Id ?? int.Parse(id);
            var parentId = sourceData?.ParentTagId > 0 ? sourceData.ParentTagId.ToString() : "0";
            var enabledVal = sourceData?.Enabled;
            var enabled = (enabledVal is bool b && b) || (enabledVal?.ToString()?.ToLower() == "yes") ? "Yes" : "No";

            var listItemModel = new ListItemModel
            {
                Id = tagId,
                Value = sourceData?.Value ?? "",
                ParentId = parentId,
                Enabled = enabled
            };

            var data = JsonConvert.SerializeObject(listItemModel);
            var returnId = await _listRepository.EditTableEntryAsync(data);

            _logWriter.LogInfo($"EditTag: TagId={tagId}, Name={listItemModel.Value}, ParentTagId={parentId}", nameof(EditTagEvent), nameof(RunAsync));

            return returnId;
        }

        /// <summary>
        /// Model for the mapped edit tag form output.
        /// </summary>
        private class EditTagMappedModel
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public int ParentTagId { get; set; }
            public object Enabled { get; set; }
        }
    }
}
