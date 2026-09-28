using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.List;

/// <summary>
/// Handles the Edit Table Entry event: reads crafted UI payload, maps it to a ListItemModel,
/// and calls the repository to persist changes (including CSV parent IDs).
/// </summary>
internal class EditTableEntryEvent : IRun
{
    private readonly IListRepository _listRepository;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Creates an instance of the EditTableEntryEvent handler.
    /// </summary>
    /// <param name="listRepository">Repository used to persist the edited list item.</param>
    /// <param name="logWriter">Logger used to record the parent ids actually persisted.</param>
    public EditTableEntryEvent(IListRepository listRepository, ILogWriter logWriter)
    {
        _listRepository = listRepository;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Parses the crafted payload and invokes the repository to update a list item.
    /// </summary>
    /// <param name="dataToSave">Serialized crafted pages payload containing field values.</param>
    /// <param name="id">Not used for this event (id provided in the crafted payload).</param>
    /// <param name="command">Event metadata.</param>
    /// <param name="eventData">Optional event configuration.</param>
    /// <returns>The edited list item id.</returns>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var sourceData = JsonConvert.DeserializeObject<TableEntryCraftedModel>(dataToSave);
        var contents = sourceData.Crafted.First().Contents;

        var listItemModel = new ListItemModel
        {
            Id = int.Parse(sourceData.Id),
            Value = ValueFor(contents, TableEntryFieldKeys.Value),
            Enabled = ValueFor(contents, TableEntryFieldKeys.Enabled),
            ParentId = ValueFor(contents, TableEntryFieldKeys.ParentId) ?? string.Empty
        };

        _logWriter.LogInfo(
            $"Editing table entry {listItemModel.Id} with parentIds='{listItemModel.ParentId}'",
            nameof(EditTableEntryEvent),
            nameof(RunAsync));

        var data = JsonConvert.SerializeObject(listItemModel);

        return await _listRepository.EditTableEntryAsync(data);
    }

    /// <summary>
    /// Reads a value out of the crafted contents by key, returning null when the key is absent.
    /// </summary>
    /// <param name="contents">The crafted contents submitted by the form.</param>
    /// <param name="key">The key to read, from <see cref="TableEntryFieldKeys"/>.</param>
    /// <returns>The value held against the key, or null.</returns>
    private static string ValueFor(List<KeyValueModel> contents, string key)
    {
        return contents.FirstOrDefault((r) => r.Key == key)?.Value;
    }
}
