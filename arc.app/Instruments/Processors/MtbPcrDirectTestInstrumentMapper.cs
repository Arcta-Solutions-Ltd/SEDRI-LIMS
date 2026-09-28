using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models.Instruments;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Instruments.Processors;

/// <summary>
/// Maps GeneXpert-style <see cref="DirectTestInstrumentFileModel"/> payloads into the pre-mapping JSON expected by the <c>mtbpcrform</c> / <c>mtbpcrformmapping</c> pipeline.
/// </summary>
internal static class MtbPcrDirectTestInstrumentMapper
{
    /// <summary>List id for MTB / RIF main result (MTB PCR).</summary>
    internal const int MtbPcrMainResultListId = 141;

    /// <summary>List id for analyte result (MTB PCR Analyte Results).</summary>
    internal const int MtbPcrAnalyteResultListId = 142;

    /// <summary>
    /// Builds the JSON body for <see cref="IRunEventHandler.RunAsync"/> with <c>Event</c> = mtbpcrform.
    /// </summary>
    public static async Task<JObject> BuildMtbPcrSavePayloadAsync(
        DirectTestInstrumentFileModel file,
        int testId,
        IListRepository listRepository,
        ILogger logger)
    {
        var mtbAssay = file.Assays?.FirstOrDefault(a =>
            string.Equals(a.AssayName?.Trim(), "MTB", StringComparison.OrdinalIgnoreCase));
        var rifAssay = file.Assays?.FirstOrDefault(a =>
            string.Equals(a.AssayName?.Trim(), "RIF Resistance", StringComparison.OrdinalIgnoreCase));

        if (mtbAssay == null)
            throw new InvalidOperationException("Assays must include an assay with AssayName 'MTB'.");
        if (rifAssay == null)
            throw new InvalidOperationException("Assays must include an assay with AssayName 'RIF Resistance'.");

        var mtbMainId = await ResolveMainResultListItemIdAsync(listRepository, mtbAssay.MainResult, logger);
        var rifMainId = await ResolveMainResultListItemIdAsync(listRepository, rifAssay.MainResult, logger);

        var mtbGrid = new JArray();
        foreach (var row in mtbAssay.AnalyteResults ?? new List<DirectTestInstrumentAnalyteResultModel>())
        {
            var analyteId = await ResolveAnalyteResultListItemIdAsync(listRepository, row.Result, logger);
            mtbGrid.Add(new JObject
            {
                ["AnalyteName"] = row.Analyte ?? "",
                ["CtValue"] = row.CtValue ?? "",
                ["EndPtValue"] = row.EndPtValue ?? "",
                ["AnalyteResult"] = analyteId.ToString()
            });
        }

        var rifGrid = new JArray();
        foreach (var row in rifAssay.AnalyteResults ?? new List<DirectTestInstrumentAnalyteResultModel>())
        {
            var analyteId = await ResolveAnalyteResultListItemIdAsync(listRepository, row.Result, logger);
            rifGrid.Add(new JObject
            {
                ["AnalyteName0"] = row.Analyte ?? "",
                ["CtValue0"] = row.CtValue ?? "",
                ["EndPtValue0"] = row.EndPtValue ?? "",
                ["AnalyteResult0"] = analyteId.ToString()
            });
        }

        return new JObject
        {
            ["Event"] = "mtbpcrform",
            ["Id"] = testId.ToString(),
            ["printonreport"] = "Yes",
            ["MTB0Id"] = mtbMainId.ToString(),
            ["MTBAnalyteResults0"] = mtbGrid,
            ["RIFResistance0Id"] = rifMainId.ToString(),
            ["RIFResistanceAnalyteResults0"] = rifGrid
        };
    }

    private static async Task<int> ResolveMainResultListItemIdAsync(IListRepository listRepository, string rawValue, ILogger logger)
    {
        var normalized = rawValue.NormalizeInstrumentListValueForMtbPcr();
        if (string.IsNullOrEmpty(normalized))
            throw new InvalidOperationException("MainResult is required for MTB PCR assays.");

        var id = await listRepository.GetListIdFromValueAsync(normalized, MtbPcrMainResultListId);
        if (id > 0)
            return id;

        logger.LogWarning("MTB PCR main result list lookup failed for value {Value}; retrying trimmed.", rawValue);
        id = await listRepository.GetListIdFromValueAsync(normalized.Trim(), MtbPcrMainResultListId);
        if (id > 0)
            return id;

        throw new InvalidOperationException(
            $"No listitem on MTB PCR list ({MtbPcrMainResultListId}) matches MainResult '{rawValue}' (normalized '{normalized}').");
    }

    private static async Task<int> ResolveAnalyteResultListItemIdAsync(IListRepository listRepository, string rawValue, ILogger logger)
    {
        var normalized = rawValue.NormalizeInstrumentListValueForMtbPcr();
        if (string.IsNullOrEmpty(normalized))
            throw new InvalidOperationException("Analyte Result is required for each analyte row.");

        var id = await listRepository.GetListIdFromValueAsync(normalized, MtbPcrAnalyteResultListId);
        if (id > 0)
            return id;

        if (string.Equals(normalized, "N/A", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(normalized, "NA", StringComparison.OrdinalIgnoreCase))
        {
            id = await listRepository.GetListIdFromValueAsync("Invalid", MtbPcrAnalyteResultListId);
            if (id > 0)
            {
                logger.LogInformation("Mapped analyte result N/A/NA to Invalid list item for MTB PCR analyte list.");
                return id;
            }
        }

        throw new InvalidOperationException(
            $"No listitem on MTB PCR analyte list ({MtbPcrAnalyteResultListId}) matches Result '{rawValue}'.");
    }
}
