using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.List
{
    /// <summary>
    /// Handles the Add Table Entry event. The add form is mapped to a flat table entry by addtableentrymapper,
    /// so this event reads the mapped values rather than the crafted contents.
    /// </summary>
    internal class AddTableEntryEvent : IRun
    {
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Creates an instance of the AddTableEntryEvent handler.
        /// </summary>
        /// <param name="listRepository">Repository used to persist the new list item.</param>
        /// <param name="logWriter">Logger used to record the parent ids actually persisted.</param>
        public AddTableEntryEvent(IListRepository listRepository, ILogWriter logWriter)
        {
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Maps the submitted table entry to a list item and invokes the repository to insert it.
        /// </summary>
        /// <param name="dataToSave">Serialized table entry produced by addtableentrymapper.</param>
        /// <param name="id">Not used for this event.</param>
        /// <param name="command">Event metadata.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>The new list item id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var sourceData = JsonConvert.DeserializeObject<TableEntryModel>(dataToSave);

            var listItemModel = new ListItemModel
            {
                ListId = sourceData.ListId,
                Value = sourceData.Value,
                Enabled = sourceData.Enabled,
                Fixed = sourceData.Fixed ? "Yes" : "No",
                ParentId = sourceData.ParentId
            };

            listItemModel.Enabled = listItemModel.Enabled == null ? "Yes" : listItemModel.Enabled;

            _logWriter.LogInfo(
                $"Adding table entry to list {listItemModel.ListId} with parentIds='{listItemModel.ParentId}'",
                nameof(AddTableEntryEvent),
                nameof(RunAsync));

            var data = JsonConvert.SerializeObject(listItemModel);

            return await _listRepository.AddTableEntryAsync(data);
        }
    }
}
