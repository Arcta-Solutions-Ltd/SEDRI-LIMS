using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.List
{
    /// <summary>
    /// Handles the addtag special event. Creates a new tag (ListItem with ListId=105)
    /// and optionally links it to a parent tag via listitemparentchild.
    /// </summary>
    internal class AddTagEvent : IRun
    {
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTagEvent"/> class.
        /// </summary>
        /// <param name="listRepository">The list repository for persisting tags.</param>
        /// <param name="logWriter">The log writer for debugging.</param>
        public AddTagEvent(IListRepository listRepository, ILogWriter logWriter)
        {
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Runs the add tag event. Deserializes the mapped data, builds a ListItemModel,
        /// and persists via AddTableEntryAsync (which handles listitemparentchild).
        /// </summary>
        /// <param name="dataToSave">JSON with Value (tag name), ParentTagId (optional parent), ListId, etc.</param>
        /// <param name="id">Not used for add; pass "0".</param>
        /// <param name="command">The event command.</param>
        /// <param name="eventData">The event configuration.</param>
        /// <returns>The newly created tag Id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sourceData = JsonConvert.DeserializeObject<AddTagMappedModel>(dataToSave);
            var parentId = sourceData?.ParentTagId > 0 ? sourceData.ParentTagId.ToString() : "0";

            var listItemModel = new ListItemModel
            {
                ListId = 105,
                Value = sourceData?.Value ?? "",
                ParentId = parentId,
                Enabled = "Yes",
                Fixed = "No"
            };

            var data = JsonConvert.SerializeObject(listItemModel);
            var returnId = await _listRepository.AddTableEntryAsync(data);

            _logWriter.LogInfo($"AddTag: Name={listItemModel.Value}, ParentTagId={parentId}, new Id={returnId}", nameof(AddTagEvent), nameof(RunAsync));

            return returnId;
        }

        /// <summary>
        /// Model for the mapped add tag form output.
        /// </summary>
        private class AddTagMappedModel
        {
            public string Value { get; set; }
            public int ParentTagId { get; set; }
        }
    }
}
