using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Executes the Turn Around Time form initial query. Loads TAT config from laboratoryconfigs,
/// converts storage format to form grid format, and enriches with DirectTestsUsedInLab and CultureTestsUsedInLab.
/// </summary>
internal class TurnAroundTimeFormInitialQueryHandler : IQueryRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="TurnAroundTimeFormInitialQueryHandler"/> class.
    /// </summary>
    /// <param name="serviceProvider">Provides access to application services.</param>
    public TurnAroundTimeFormInitialQueryHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Runs the query to retrieve TAT configuration for a laboratory.
    /// </summary>
    /// <param name="queryFilter">Filters containing laboratory id.</param>
    /// <param name="token">Authentication or session token information.</param>
    /// <returns>A JSON-formatted string containing the TAT form data.</returns>
    public async Task<string> RunAsync(QueryFilterConfig queryFilter, TokenInfoModel token)
    {
        var labId = queryFilter.GetStringValue("id");
        if (string.IsNullOrEmpty(labId))
        {
            return "{}";
        }

        var laboratoryConfigRepository = _serviceProvider.GetService<ILaboratoryConfigRepository>();
        var labIdInt = int.Parse(labId);
        var queryFilterConfig = new QueryFilterConfig("id", labId, "configname", "TurnAroundTime");
        var configs = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilterConfig);

        var emptyConfig = new Dictionary<string, object>
        {
            ["Id"] = labIdInt,
            ["LaboratoryId"] = labIdInt,
            ["SpecimenRangeGrid"] = Array.Empty<object>(),
            ["DirectTestDefaultRangeGrid"] = Array.Empty<object>(),
            ["DirectTestOverrideGrid"] = Array.Empty<object>(),
            ["CultureTestDefaultRangeGrid"] = Array.Empty<object>(),
            ["CultureTestOverrideGrid"] = Array.Empty<object>(),
            ["DirectTestsUsedInLab"] = await GetDirectTestsUsedInLaboratoryAsync(laboratoryConfigRepository, labId),
            ["CultureTestsUsedInLab"] = await GetCultureTestsUsedInLaboratoryAsync(laboratoryConfigRepository, labId)
        };

        if (configs == null || configs.Count == 0)
        {
            return JsonConvert.SerializeObject(emptyConfig);
        }

        var first = configs[0];
        var contents = string.IsNullOrEmpty(first.Contents) ? "{}" : first.Contents;
        var merged = JsonConvert.DeserializeObject<Dictionary<string, object>>(contents) ?? new Dictionary<string, object>();
        merged["Id"] = first.LaboratoryId;
        merged["LaboratoryId"] = first.LaboratoryId;
        if (!merged.ContainsKey("SpecimenRangeGrid") && merged.ContainsKey("specimenRanges"))
        {
            merged["SpecimenRangeGrid"] = MapStorageRangesToFormFormat(merged["specimenRanges"]);
        }
        if (!merged.ContainsKey("DirectTestDefaultRangeGrid") && merged.ContainsKey("directTestDefaultRanges"))
        {
            merged["DirectTestDefaultRangeGrid"] = MapStorageRangesToFormFormat(merged["directTestDefaultRanges"]);
        }
        if (!merged.ContainsKey("DirectTestOverrideGrid") && merged.ContainsKey("directTestOverrides"))
        {
            merged["DirectTestOverrideGrid"] = FlattenOverrideToGrid(merged["directTestOverrides"]);
        }
        if (!merged.ContainsKey("CultureTestDefaultRangeGrid") && merged.ContainsKey("cultureTestDefaultRanges"))
        {
            merged["CultureTestDefaultRangeGrid"] = MapStorageRangesToFormFormat(merged["cultureTestDefaultRanges"]);
        }
        if (!merged.ContainsKey("CultureTestOverrideGrid") && merged.ContainsKey("cultureTestOverrides"))
        {
            merged["CultureTestOverrideGrid"] = FlattenOverrideToGrid(merged["cultureTestOverrides"]);
        }
        merged["DirectTestsUsedInLab"] = await GetDirectTestsUsedInLaboratoryAsync(laboratoryConfigRepository, labId);
        merged["CultureTestsUsedInLab"] = await GetCultureTestsUsedInLaboratoryAsync(laboratoryConfigRepository, labId);
        return JsonConvert.SerializeObject(merged);
    }

    private static async Task<List<object>> GetDirectTestsUsedInLaboratoryAsync(ILaboratoryConfigRepository laboratoryConfigRepository, string labId)
    {
        var formNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var configName in new[] { "specimentypedirecttestdefault", "specimentypedirecttestoption" })
        {
            var queryFilter = new QueryFilterConfig("id", labId, "configname", configName);
            var configs = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilter);
            if (configs != null)
            {
                foreach (var c in configs)
                {
                    if (string.IsNullOrEmpty(c.Contents)) continue;
                    try
                    {
                        var parsed = JsonConvert.DeserializeObject<dynamic>(c.Contents);
                        var associatedListId = (string)(parsed?.AssociatedListId ?? "");
                        if (!string.IsNullOrEmpty(associatedListId))
                        {
                            foreach (var name in associatedListId.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                            {
                                formNames.Add(name.Trim());
                            }
                        }
                    }
                    catch { /* ignore parse errors */ }
                }
            }
        }
        return formNames.OrderBy(x => x).Select(x => (object)new { id = x, value = x }).ToList();
    }

    private static async Task<List<object>> GetCultureTestsUsedInLaboratoryAsync(ILaboratoryConfigRepository laboratoryConfigRepository, string labId)
    {
        var formNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var configName in new[] { "culturetypeculturetestdefault", "culturetypeculturetestoption" })
        {
            var queryFilter = new QueryFilterConfig("id", labId, "configname", configName);
            var configs = await laboratoryConfigRepository.GetLaboratoryConfigListAsync(queryFilter);
            if (configs != null)
            {
                foreach (var c in configs)
                {
                    if (string.IsNullOrEmpty(c.Contents)) continue;
                    try
                    {
                        var parsed = JsonConvert.DeserializeObject<dynamic>(c.Contents);
                        var associatedListId = (string)(parsed?.AssociatedListId ?? "");
                        if (!string.IsNullOrEmpty(associatedListId))
                        {
                            foreach (var name in associatedListId.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                            {
                                formNames.Add(name.Trim());
                            }
                        }
                    }
                    catch { /* ignore parse errors */ }
                }
            }
        }
        return formNames.OrderBy(x => x).Select(x => (object)new { id = x, value = x }).ToList();
    }

    private static object MapStorageRangesToFormFormat(object rangesObj)
    {
        if (rangesObj == null) return Array.Empty<object>();
        var ranges = rangesObj as Newtonsoft.Json.Linq.JArray;
        if (ranges == null) return Array.Empty<object>();
        var result = new List<object>();
        foreach (var r in ranges)
        {
            var row = new Dictionary<string, object>();
            if (r["minDays"] != null || r["days"] != null)
            {
                row["RangeFromDays"] = r["minDays"] != null ? (int)r["minDays"] : 0;
                row["RangeFromHours"] = r["minHours"] != null ? (int)r["minHours"] : 0;
                row["RangeFromMinutes"] = r["minMinutes"] != null ? (int)r["minMinutes"] : 0;
                row["RangeToDays"] = r["maxDays"] != null ? (int)r["maxDays"] : (r["days"] != null ? (int)r["days"] : 0);
                row["RangeToHours"] = r["maxHours"] != null ? (int)r["maxHours"] : (r["hours"] != null ? (int)r["hours"] : 0);
                row["RangeToMinutes"] = r["maxMinutes"] != null ? (int)r["maxMinutes"] : (r["minutes"] != null ? (int)r["minutes"] : 0);
            }
            if (r["colour"] != null) row["Colour"] = r["colour"].ToString();
            result.Add(row);
        }
        return result;
    }

    private static object FlattenOverrideToGrid(object overridesObj)
    {
        if (overridesObj == null) return Array.Empty<object>();
        var overrides = overridesObj as Newtonsoft.Json.Linq.JObject;
        if (overrides == null) return Array.Empty<object>();
        var result = new List<object>();
        foreach (var kv in overrides)
        {
            var testName = kv.Key;
            var ranges = kv.Value as Newtonsoft.Json.Linq.JArray;
            if (ranges == null) continue;
            foreach (var r in ranges)
            {
                var row = new Dictionary<string, object> { ["TestName"] = testName };
                if (r["minDays"] != null || r["days"] != null)
                {
                    row["RangeFromDays"] = r["minDays"] != null ? (int)r["minDays"] : 0;
                    row["RangeFromHours"] = r["minHours"] != null ? (int)r["minHours"] : 0;
                    row["RangeFromMinutes"] = r["minMinutes"] != null ? (int)r["minMinutes"] : 0;
                    row["RangeToDays"] = r["maxDays"] != null ? (int)r["maxDays"] : (r["days"] != null ? (int)r["days"] : 0);
                    row["RangeToHours"] = r["maxHours"] != null ? (int)r["maxHours"] : (r["hours"] != null ? (int)r["hours"] : 0);
                    row["RangeToMinutes"] = r["maxMinutes"] != null ? (int)r["maxMinutes"] : (r["minutes"] != null ? (int)r["minutes"] : 0);
                }
                if (r["colour"] != null) row["Colour"] = r["colour"].ToString();
                result.Add(row);
            }
        }
        return result;
    }
}
