using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Config.Forms;
using arc.app.Config.Pages;
using arc.domain.Configuration.BarcodeConfig;
using arc.domain.Configuration.FormsConfig;
using arc.domain.Configuration.PagesConfig;
using Microsoft.Extensions.Logging;

namespace arc.app.Config.Barcode;

/// <summary>
/// Resolves barcode label captions using form definitions on the server, mirroring portal GetFieldsForForm traversal for known layouts.
/// </summary>
/// <remarks>
/// Excludes nested fieldgrid definitions from caption lookup (parity with portal GetFieldsForForm default exclude).
/// </remarks>
public class BarcodePrintLabelResolver(
    IFormConfigAdapter formConfigAdapter,
    IPageConfigAdapter pageConfigAdapter,
    ILogger<BarcodePrintLabelResolver> logger)
    : IBarcodePrintLabelResolver
{
    private const string DefaultAccessionCaption = "@SpeAcc@";

    /// <inheritdoc />
    public async Task EnrichBarcodePrintConfigsAsync(IReadOnlyList<BarcodePrintConfig> configs, string username)
    {
        if (configs == null || configs.Count == 0)
        {
            return;
        }

        var formCaptionCache = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var config in configs)
        {
            if (config == null)
            {
                continue;
            }

            var requestedIds = ParseLabelFieldIds(config.LabelFields);
            if (requestedIds.Count == 0)
            {
                config.LabelCaptions ??= [];
                continue;
            }

            config.LabelCaptions ??= [];

            NormalizeExistingCaptionKeys(config.LabelCaptions);

            var missingIds = requestedIds.Where(id => !config.LabelCaptions.ContainsKey(id)).ToList();
            if (missingIds.Count == 0)
            {
                continue;
            }

            if (string.IsNullOrWhiteSpace(config.ReferenceForm))
            {
                await ApplyFallbacksWithoutFormAsync(config, missingIds, username);
                continue;
            }

            if (!formCaptionCache.TryGetValue(config.ReferenceForm, out var fieldCaptionsById))
            {
                fieldCaptionsById = await BuildCaptionMapAsync(config.ReferenceForm);
                formCaptionCache[config.ReferenceForm] = fieldCaptionsById;
            }

            foreach (var fieldId in missingIds)
            {
                if (string.Equals(fieldId, "accessionnumber", StringComparison.OrdinalIgnoreCase))
                {
                    config.LabelCaptions[fieldId] = DefaultAccessionCaption;
                    continue;
                }

                if (fieldCaptionsById.TryGetValue(fieldId, out var caption) && !string.IsNullOrWhiteSpace(caption))
                {
                    config.LabelCaptions[fieldId] = caption;
                }
                else
                {
                    config.LabelCaptions[fieldId] = HumanizeMissingFieldCaption(fieldId);
                    logger.LogWarning(
                        "Barcode label resolver: unknown field {FieldId} for layout {BarcodeName} form {ReferenceForm} user {Username}",
                        fieldId,
                        config.Name ?? "",
                        config.ReferenceForm ?? "",
                        username ?? "");
                }
            }
        }
    }

    private static void NormalizeExistingCaptionKeys(Dictionary<string, string> captions)
    {
        if (captions == null || captions.Count == 0)
        {
            return;
        }

        foreach (var key in captions.Keys.ToList())
        {
            var lower = key.ToLowerInvariant();
            if (!string.Equals(key, lower, StringComparison.Ordinal) && captions.TryGetValue(key, out var value))
            {
                captions.Remove(key);
                if (!captions.ContainsKey(lower))
                {
                    captions[lower] = value;
                }
            }
        }
    }

    private static List<string> ParseLabelFieldIds(string labelFieldsCsv)
    {
        if (string.IsNullOrWhiteSpace(labelFieldsCsv))
        {
            return [];
        }

        return labelFieldsCsv
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim().ToLowerInvariant())
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private async Task<IReadOnlyDictionary<string, string>> BuildCaptionMapAsync(string referenceFormName)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        FormConfig form;

        try
        {
            form = await formConfigAdapter.GetFormAsync(referenceFormName);
        }
        catch (Exception ex)
        {
            logger.LogWarning(
                ex,
                "Barcode label resolver failed to load form {FormName}",
                referenceFormName);
            return map;
        }

        if (form?.Pages == null || form.Pages.Count == 0)
        {
            return map;
        }

        foreach (var pageName in form.Pages)
        {
            if (string.IsNullOrWhiteSpace(pageName))
            {
                continue;
            }

            PageConfig pageCfg;
            try
            {
                pageCfg = await pageConfigAdapter.GetPageAsync(pageName);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Barcode label resolver failed to load page {PageName} for form {FormName}",
                    pageName,
                    referenceFormName);
                continue;
            }

            if (pageCfg?.Columns == null)
            {
                continue;
            }

            foreach (var column in pageCfg.Columns.Where(c => c?.FormGroups != null))
            {
                foreach (var group in column.FormGroups)
                {
                    if (group?.Fields == null)
                    {
                        continue;
                    }

                    foreach (var field in group.Fields)
                    {
                        if (field == null || string.IsNullOrWhiteSpace(field.Id))
                        {
                            continue;
                        }

                        if (string.Equals(field.Type, "fieldgrid", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var idKey = field.Id.ToLowerInvariant();
                        if (!map.ContainsKey(idKey) && !string.IsNullOrWhiteSpace(field.Label))
                        {
                            map[idKey] = field.Label!;
                        }
                    }
                }
            }
        }

        return map;
    }

    /// <summary>
    /// Applies known captions when no reference form path is usable.
    /// </summary>
    private Task ApplyFallbacksWithoutFormAsync(BarcodePrintConfig config, List<string> missingIds, string username)
    {
        foreach (var fieldId in missingIds)
        {
            if (string.Equals(fieldId, "accessionnumber", StringComparison.OrdinalIgnoreCase))
            {
                config.LabelCaptions![fieldId] = DefaultAccessionCaption;
                continue;
            }

            config.LabelCaptions![fieldId] = HumanizeMissingFieldCaption(fieldId);
            logger.LogWarning(
                "Barcode label resolver has no ReferenceForm for layout {BarcodeName}; caption for {FieldId} defaulted for user {Username}",
                config.Name ?? "",
                fieldId,
                username ?? "");
        }

        return Task.CompletedTask;
    }

    private static string HumanizeMissingFieldCaption(string fieldId)
    {
        if (string.IsNullOrWhiteSpace(fieldId))
        {
            return fieldId;
        }

        // Simple split on capital transitions for readability if no translation tag exists
        var chars = fieldId.Trim().ToCharArray();
        if (chars.Length > 1)
        {
            var result = chars[0].ToString().ToUpperInvariant();
            for (var i = 1; i < chars.Length; i++)
            {
                if (char.IsUpper(chars[i]) && char.IsLower(chars[i - 1]))
                {
                    result += " ";
                }

                result += chars[i];
            }

            return result;
        }

        return char.ToUpperInvariant(chars[0]) + fieldId[1..].ToLowerInvariant();
    }
}
