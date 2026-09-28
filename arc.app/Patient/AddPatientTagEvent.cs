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

namespace arc.app.Patient;

/// <summary>
/// Special event for adding patient tags. Supports multi-select (comma-separated TagId), single tag, and creating a new tag by name.
/// When AddOnly is true (batch add), uses AddPatientTagsAsync to skip duplicates.
/// </summary>
internal class AddPatientTagEvent : IRun
{
    private const int TagListId = 105;

    private readonly IServiceProvider _serviceProvider;

    public AddPatientTagEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var listRepository = _serviceProvider.GetService<IListRepository>();
        var genericRepository = _serviceProvider.GetService<IGenericRepository>();
        var patientRepository = _serviceProvider.GetService<IPatientRepository>();

        var request = JsonConvert.DeserializeObject<AddTagRequestModel>(dataToSave);
        var recordId = (request?.Id ?? 0) > 0 ? request.Id.ToString() : id;
        var patientId = int.Parse(recordId);
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

        if (tagIds.Count == 0)
            throw new ArgumentException("@GenTagJ@");

        if (request?.AddOnly == true)
        {
            return await patientRepository.AddPatientTagsAsync(patientId, tagIds);
        }

        int? lastReturnId = null;
        foreach (var listItemId in tagIds)
        {
            var payload = JsonConvert.SerializeObject(new { PatientId = patientId, ListItemId = listItemId });
            genericRepository.AddConfiguration("PatientTag");
            lastReturnId = await genericRepository.AddAsync(payload, command, eventData?.StringFields ?? "");
        }

        return lastReturnId ?? 0;
    }
}
