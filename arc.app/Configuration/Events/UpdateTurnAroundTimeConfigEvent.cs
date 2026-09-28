using arc.app.Common;
using arc.app.SystemConfig;
using arc.common;
using arc.data.model.Configuration;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Configuration.Events;

/// <summary>
/// Event for updating Turn Around Time configuration for a laboratory.
/// </summary>
internal class UpdateTurnAroundTimeConfigEvent : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTurnAroundTimeConfigEvent"/> class.
    /// </summary>
    public UpdateTurnAroundTimeConfigEvent(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the event to save Turn Around Time config to laboratoryconfigs.
    /// </summary>
    public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
    {
        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();

        var labId = id;
        if (string.IsNullOrEmpty(labId))
        {
            var parsed = JObject.Parse(dataToSave);
            labId = parsed["LaboratoryId"]?.ToString() ?? parsed["Id"]?.ToString();
        }

        if (string.IsNullOrEmpty(labId))
        {
            return -1;
        }

        var queryFilter = new QueryFilterConfig("id", labId, "configname", "TurnAroundTime");
        var configs = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilter);

        var contents = TransformFormDataToStorage(dataToSave);

        if (configs != null && configs.Count > 0)
        {
            var existing = configs[0];
            existing.Contents = contents;
            await laboratoryConfigRepository.UpdateAsync(existing, "LaboratoryConfigs", "id");
        }
        else
        {
            var newRecord = new LaboratoryConfigsDataModel
            {
                ConfigName = "TurnAroundTime",
                LaboratoryId = int.Parse(labId),
                Contents = contents
            };
            await laboratoryConfigRepository.AddAsync(newRecord, "LaboratoryConfigs");
        }

        return 0;
    }

    private static string TransformFormDataToStorage(string dataToSave)
    {
        var obj = JObject.Parse(dataToSave);
        var storage = new Dictionary<string, object>();

        if (obj["SpecimenRangeGrid"] is JArray specimenRanges)
        {
            storage["specimenRanges"] = specimenRanges.Select(r => MapRangeToStorage(r)).ToArray();
        }
        else
        {
            storage["specimenRanges"] = Array.Empty<object>();
        }

        if (obj["DirectTestDefaultRangeGrid"] is JArray directDefaultRanges)
        {
            storage["directTestDefaultRanges"] = directDefaultRanges.Select(r => MapRangeToStorage(r)).ToArray();
        }
        else
        {
            storage["directTestDefaultRanges"] = Array.Empty<object>();
        }

        storage["directTestOverrides"] = GroupOverrideGridByTestName(obj["DirectTestOverrideGrid"]);

        if (obj["CultureTestDefaultRangeGrid"] is JArray cultureDefaultRanges)
        {
            storage["cultureTestDefaultRanges"] = cultureDefaultRanges.Select(r => MapRangeToStorage(r)).ToArray();
        }
        else
        {
            storage["cultureTestDefaultRanges"] = Array.Empty<object>();
        }

        storage["cultureTestOverrides"] = GroupOverrideGridByTestName(obj["CultureTestOverrideGrid"]);

        return JsonConvert.SerializeObject(storage);
    }

    private static Dictionary<string, object> GroupOverrideGridByTestName(JToken overrideGrid)
    {
        var result = new Dictionary<string, object>();
        if (overrideGrid is not JArray rows) return result;

        foreach (var row in rows)
        {
            var testName = row["TestName"]?.ToString();
            if (string.IsNullOrEmpty(testName)) continue;

            var range = MapRangeToStorage(row);
            if (!result.ContainsKey(testName))
            {
                result[testName] = new List<object>();
            }
            ((List<object>)result[testName]).Add(range);
        }
        return result;
    }

    private static int GetIntOrDefault(JToken token, int defaultValue)
    {
        if (token == null || string.IsNullOrWhiteSpace(token.ToString()))
            return defaultValue;
        return int.TryParse(token.ToString(), out var v) ? v : defaultValue;
    }

    private static object MapRangeToStorage(JToken range)
    {
        var dict = new Dictionary<string, object>();
        int minDays, minHours, minMinutes, maxDays, maxHours, maxMinutes;

        if (range["RangeFromDays"] != null || range["RangeToDays"] != null)
        {
            minDays = GetIntOrDefault(range["RangeFromDays"], 0);
            minHours = GetIntOrDefault(range["RangeFromHours"], 0);
            minMinutes = GetIntOrDefault(range["RangeFromMinutes"], 0);
            maxDays = GetIntOrDefault(range["RangeToDays"], 0);
            maxHours = GetIntOrDefault(range["RangeToHours"], 0);
            maxMinutes = GetIntOrDefault(range["RangeToMinutes"], 0);
        }
        else
        {
            var days = GetIntOrDefault(range["Days"], 0);
            var hours = GetIntOrDefault(range["Hours"], 0);
            var minutes = GetIntOrDefault(range["Minutes"], 0);
            minDays = minHours = minMinutes = 0;
            maxDays = days;
            maxHours = hours;
            maxMinutes = minutes;
        }

        dict["minDays"] = minDays;
        dict["minHours"] = minHours;
        dict["minMinutes"] = minMinutes;
        dict["maxDays"] = maxDays;
        dict["maxHours"] = maxHours;
        dict["maxMinutes"] = maxMinutes;
        if (range["Colour"] != null) dict["colour"] = range["Colour"].ToString();
        return dict;
    }
}
