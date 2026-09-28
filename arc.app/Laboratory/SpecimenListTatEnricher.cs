using arc.app.Specimen;
using arc.app.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Enriches specimen list JSON with TurnAroundTimeColour from lab TAT config.
/// Default mode (active list): end time from finalisation history when state is finalised (534), otherwise elapsed to now.
/// Archive mode: end time from the last SpecimenStateHistory row matching the specimen's current terminal state (534, 528, 537).
/// </summary>
public class SpecimenListTatEnricher : ISpecimenListTatEnricher
{
    private readonly ILaboratoryConfigRepository _labConfigRepository;
    private readonly ISpecimenRepository _specimenRepository;

    public SpecimenListTatEnricher(ILaboratoryConfigRepository labConfigRepository, ISpecimenRepository specimenRepository)
    {
        _labConfigRepository = labConfigRepository;
        _specimenRepository = specimenRepository;
    }

    public async Task<string> EnrichAsync(string specimenListJson, bool archiveList = false)
    {
        if (string.IsNullOrEmpty(specimenListJson)) return specimenListJson;

        var items = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(specimenListJson);
        if (items == null || items.Count == 0) return specimenListJson;

        var specimenIds = items
            .Select(x => GetInt(x, "id") ?? GetInt(x, "Id"))
            .Where(x => x.HasValue && x.Value > 0)
            .Select(x => x.Value)
            .Distinct()
            .ToList();

        var finalisedDates = archiveList
            ? null
            : await _specimenRepository.GetSpecimenFinalisedDatesAsync(specimenIds);
        var terminalStateEndTimes = archiveList
            ? await _specimenRepository.GetSpecimenTerminalStateEndTimesAsync(specimenIds)
            : null;

        var labIds = items
            .Select(x => GetInt(x, "laboratoryid") ?? GetInt(x, "LaboratoryId"))
            .Where(x => x.HasValue && x.Value > 0)
            .Select(x => x.Value)
            .Distinct()
            .ToList();

        var configByLab = new Dictionary<int, JArray>();
        foreach (var labId in labIds)
        {
            var queryFilter = new QueryFilterConfig("id", labId.ToString(), "configname", "TurnAroundTime");
            var configs = await _labConfigRepository.GetLaboratoryConfigListAsync(queryFilter);
            if (configs != null && configs.Count > 0 && !string.IsNullOrEmpty(configs[0].Contents))
            {
                var config = JObject.Parse(configs[0].Contents);
                var specimenRanges = config["specimenRanges"] as JArray;
                if (specimenRanges != null)
                    configByLab[labId] = specimenRanges;
            }
        }

        foreach (var item in items)
        {
            var specimenId = GetInt(item, "id") ?? GetInt(item, "Id") ?? 0;
            var labId = GetInt(item, "laboratoryid") ?? GetInt(item, "LaboratoryId");
            if (!labId.HasValue || labId.Value <= 0 || !configByLab.TryGetValue(labId.Value, out var ranges))
                continue;

            var receivedDate = GetDateTime(item, "receiveddate") ?? GetDateTime(item, "ReceivedDate") ?? GetDateTimeFromAnyKey(item, "receiveddate", "ReceivedDate", "received_date");
            var receivedTime = GetString(item, "receivedtime") ?? GetString(item, "ReceivedTime") ?? GetStringFromAnyKey(item, "receivedtime", "ReceivedTime", "received_time");
            var currentStateId = GetInt(item, "stateid") ?? GetInt(item, "StateId");
            DateTime? finalised = null;
            if (archiveList && specimenId > 0 && terminalStateEndTimes != null)
            {
                if (terminalStateEndTimes.TryGetValue(specimenId, out var terminalEnd))
                    finalised = terminalEnd;
            }
            else if (finalisedDates != null
                && currentStateId == TurnAroundTimeCalculator.SpecimenFinalisedStateId
                && specimenId > 0
                && finalisedDates.TryGetValue(specimenId, out var fd))
            {
                finalised = fd;
            }

            var colour = TurnAroundTimeCalculator.GetSpecimenColour(receivedDate, receivedTime, finalised, ranges);

            if (colour != null)
                item["TurnAroundTimeColour"] = colour;
        }

        return JsonConvert.SerializeObject(items);
    }

    private static int? GetInt(Dictionary<string, object> d, string key)
    {
        if (!d.TryGetValue(key, out var v) || v == null) return null;
        if (v is int i) return i;
        if (v is long l) return (int)l;
        if (int.TryParse(v.ToString(), out var parsed)) return parsed;
        return null;
    }

    private static DateTime? GetDateTime(Dictionary<string, object> d, string key)
    {
        if (!d.TryGetValue(key, out var v) || v == null) return null;
        if (v is DateTime dt) return dt;
        if (v is DateTimeOffset dto) return dto.UtcDateTime;
        var s = v.ToString();
        if (DateTime.TryParse(s, out var parsed)) return parsed;
        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed)) return parsed;
        var formats = new[] { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm:ss", "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss", "dd.MM.yyyy" };
        if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed)) return parsed;
        return null;
    }

    private static string GetString(Dictionary<string, object> d, string key)
    {
        if (!d.TryGetValue(key, out var v) || v == null) return null;
        return v.ToString();
    }

    private static DateTime? GetDateTimeFromAnyKey(Dictionary<string, object> d, params string[] keys)
    {
        foreach (var key in keys)
        {
            var v = GetDateTime(d, key);
            if (v.HasValue) return v;
        }
        var match = d.Keys.FirstOrDefault(k => keys.Any(alt => string.Equals(k, alt, StringComparison.OrdinalIgnoreCase)));
        return match != null ? GetDateTime(d, match) : null;
    }

    private static string GetStringFromAnyKey(Dictionary<string, object> d, params string[] keys)
    {
        foreach (var key in keys)
        {
            var v = GetString(d, key);
            if (v != null) return v;
        }
        var match = d.Keys.FirstOrDefault(k => keys.Any(alt => string.Equals(k, alt, StringComparison.OrdinalIgnoreCase)));
        return match != null ? GetString(d, match) : null;
    }
}
