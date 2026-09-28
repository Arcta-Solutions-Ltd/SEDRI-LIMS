using arc.app.Common;
using arc.common.Models.Export;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace arc.app.Exports;

/// <summary>
/// Builds export schedule filter JSON from form criteria and maps stored keys for scheduled export runs.
/// Filter values are comma-separated list item ids (never display text).
/// </summary>
public static class ExportScheduleFilterBuilder
{
    /// <summary>
    /// Builds the filter JSON object from export schedule criteria form fields.
    /// </summary>
    /// <param name="vm">Submitted add/edit form view model.</param>
    /// <returns>Filter object with only non-empty id criteria; may be empty.</returns>
    public static JObject BuildFilterFromViewModel(ExportScheduleFormViewModel vm)
    {
        var filter = new JObject();
        TryAddFilterId(filter, "OrganismId", vm.OrganismId);
        TryAddFilterId(filter, "SpecimenTypeId", vm.SpecimenTypeId);
        TryAddFilterId(filter, "SpecimenStateId", vm.SpecimenStateId);
        TryAddFilterId(filter, "TagId", vm.TagId);
        TryAddFilterId(filter, "OrganisationId", vm.OrganisationId);
        TryAddFilterId(filter, "LocationId", vm.LocationId);
        TryAddFilterId(filter, "TestId", vm.TestId);
        if (!string.IsNullOrWhiteSpace(vm.ASTExclusive))
        {
            filter["ASTExclusive"] = vm.ASTExclusive;
        }

        return filter;
    }

    /// <summary>
    /// Maps a lower-cased schedule filter JSON property name to the query filter key expected by <c>SpecimenFilter</c> at run time.
    /// </summary>
    /// <param name="lowerCaseFilterKey">Property name from schedule filter JSON (lowercase).</param>
    /// <returns>Query filter parameter key for the export run handler.</returns>
    public static string MapFilterKeyForRunQuery(string lowerCaseFilterKey)
    {
        if (string.IsNullOrWhiteSpace(lowerCaseFilterKey))
        {
            return lowerCaseFilterKey ?? string.Empty;
        }

        return lowerCaseFilterKey.ToLowerInvariant() switch
        {
            "specimenstateid" => "stateid",
            "organisationid" => "organisationfilterid",
            _ => lowerCaseFilterKey.ToLowerInvariant()
        };
    }

    /// <summary>
    /// Logs when a saved schedule filter field contains multiple ids (for installed-system diagnosis).
    /// </summary>
    /// <param name="logWriter">Logger instance; no-op when null.</param>
    /// <param name="filter">Filter JSON about to be persisted.</param>
    /// <param name="scheduleName">Schedule name for correlation.</param>
    /// <param name="source">Calling type name for log context.</param>
    public static void LogMultiValueFilterCriteria(ILogWriter logWriter, JObject filter, string scheduleName, string source)
    {
        if (logWriter == null || filter == null || filter.Count == 0)
        {
            return;
        }

        foreach (var prop in filter.Properties())
        {
            if (string.Equals(prop.Name, "ASTExclusive", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var value = prop.Value?.ToString();
            if (string.IsNullOrWhiteSpace(value) || !value.Contains(','))
            {
                continue;
            }

            var idCount = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;
            logWriter.LogInfo(
                $"Export schedule filter saved with multiple ids: schedule={scheduleName}, field={prop.Name}, idCount={idCount}",
                source,
                nameof(LogMultiValueFilterCriteria));
        }
    }

    /// <summary>
    /// Normalizes a filter id value to a comma-separated list of numeric ids, or null when empty/invalid.
    /// </summary>
    /// <param name="value">Raw form value (single id or comma-separated ids).</param>
    /// <returns>Normalized comma-separated id string, or null when no valid ids remain.</returns>
    internal static string NormalizeFilterIdValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var ids = value
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(part => int.TryParse(part, out var id) && id > 0)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        return ids.Count > 0 ? string.Join(",", ids) : string.Empty;
    }

    private static void TryAddFilterId(JObject filter, string filterKey, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        var normalized = NormalizeFilterIdValue(value);
        if (!string.IsNullOrWhiteSpace(normalized))
        {
            filter[filterKey] = normalized;
        }
    }
}
