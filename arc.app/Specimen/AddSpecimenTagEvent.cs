using arc.app.Common;
using arc.common;
using arc.common.Models.Lists;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Special event for managing specimen tags. Replaces the specimen's tags with the set from the form.
/// TagId is the full desired set (comma-separated); TagName optionally creates and adds a new tag.
/// Removing a tag from the list removes it from the database.
/// </summary>
internal class AddSpecimenTagEvent : IRun
{
    private const int TagListId = 105;

    private readonly IServiceProvider _serviceProvider;

    public AddSpecimenTagEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var listRepository = _serviceProvider.GetService<IListRepository>();
        var genericRepository = _serviceProvider.GetService<IGenericRepository>();
        var specimenRepository = _serviceProvider.GetService<ISpecimenRepository>();

        var request = JsonConvert.DeserializeObject<AddTagRequestModel>(dataToSave);
        var recordId = (request?.Id ?? 0) > 0 ? request.Id.ToString() : id;
        var specimenId = int.Parse(recordId);
        var tagIdsRaw = request?.TagId?.Trim();
        var tagName = request?.TagName?.Trim();

        var tagIds = string.IsNullOrWhiteSpace(tagIdsRaw)
            ? new List<int>()
            : tagIdsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(s => int.TryParse(s, out var n) && n > 0 ? n : 0)
                .Where(n => n > 0)
                .Distinct()
                .ToList();

        if (!string.IsNullOrWhiteSpace(tagName))
        {
            var listItemId = await listRepository.GetListIdFromValueAsync(tagName, TagListId);
            if (listItemId <= 0)
            {
                genericRepository.AddConfiguration("ListItem");
                listItemId = await genericRepository.AddAsync(
                    JsonConvert.SerializeObject(new
                    {
                        ListId = TagListId,
                        Value = tagName,
                        Fixed = false,
                        Enabled = true,
                        Deleted = false
                    }), null, "");
            }
            if (listItemId > 0 && !tagIds.Contains(listItemId))
                tagIds.Add(listItemId);
        }

        var lastSpecimenTagId = request?.AddOnly == true
            ? await specimenRepository.AddSpecimenTagsAsync(specimenId, tagIds)
            : await specimenRepository.SetSpecimenTagsAsync(specimenId, tagIds);

        return lastSpecimenTagId;
    }
}
