using arc.app.SystemConfig;
using arc.common.Models.Tests;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.DependencyInjection;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Enriches test list results with Turn Around Time colour based on lab TAT configuration.
/// </summary>
public static class TurnAroundTimeEnricher
{
    public static async Task EnrichTestListAsync(
        IServiceProvider serviceProvider,
        List<TestListResultModel> list,
        bool isCultureTest)
    {
        if (list == null || list.Count == 0) return;

        var labConfigRepository = serviceProvider.GetService<ILaboratoryConfigRepository>();
        var labIds = list.Select(x => x.LaboratoryId ?? 0).Where(x => x > 0).Distinct().ToList();
        if (labIds.Count == 0) return;

        var configByLab = new Dictionary<int, JObject>();
        foreach (var labId in labIds)
        {
            var queryFilter = new QueryFilterConfig("id", labId.ToString(), "configname", "TurnAroundTime");
            var configs = await labConfigRepository.GetLaboratoryConfigListAsync(queryFilter);
            if (configs != null && configs.Count > 0 && !string.IsNullOrEmpty(configs[0].Contents))
            {
                configByLab[labId] = JObject.Parse(configs[0].Contents);
            }
        }

        var defaultRangesKey = isCultureTest ? "cultureTestDefaultRanges" : "directTestDefaultRanges";
        var overridesKey = isCultureTest ? "cultureTestOverrides" : "directTestOverrides";

        foreach (var row in list)
        {
            var labId = row.LaboratoryId ?? 0;
            if (!configByLab.TryGetValue(labId, out var config)) continue;

            var defaultRanges = config[defaultRangesKey] as JArray;
            var overrides = config[overridesKey] as JObject;

            row.TurnAroundTimeColour = TurnAroundTimeCalculator.GetTestColour(
                row.Requested,
                row.Completed,
                row.Status,
                row.TestName,
                isCultureTest,
                defaultRanges,
                overrides);
        }
    }
}
