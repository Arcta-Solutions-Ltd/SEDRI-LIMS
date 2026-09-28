using arc.app.Configuration;
using arc.common.Models.Config;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Specimen;

/// <summary>
/// Shared helpers for building table routing on specimen form saves.
/// </summary>
internal static class SpecimenFormSaveRouting
{
    private static readonly HashSet<string> EventOnlyTargetTables =
        new(System.StringComparer.OrdinalIgnoreCase) { "culture" };

    /// <summary>
    /// Builds merged table exceptions from page config routing and event-level exceptions
    /// for targets outside the four specimen-form page tables (e.g. Culture).
    /// </summary>
    /// <param name="routingService">Page TableName routing service.</param>
    /// <param name="dataToSave">Serialised save payload.</param>
    /// <param name="eventData">Event configuration.</param>
    /// <returns>Merged routing list for GenerateMoreData.</returns>
    public static async Task<List<TableExceptionModel>> BuildTableExceptionsAsync(
        IFormPageTableRoutingService routingService,
        string dataToSave,
        EventConfig eventData)
    {
        var formName = ReadFormNameFromPayload(dataToSave);
        var pageRouting = await routingService.BuildRoutingAsync(formName);
        var merged = pageRouting.ToList();
        AppendEventOnlyExceptions(merged, eventData);
        return merged;
    }

    /// <summary>
    /// Reads FormName from the save payload using JSON parsing so nested arrays (e.g. Crafted) do not break extraction.
    /// </summary>
    private static string ReadFormNameFromPayload(string dataToSave)
    {
        if (string.IsNullOrWhiteSpace(dataToSave))
        {
            return string.Empty;
        }

        try
        {
            var formNameToken = JObject.Parse(dataToSave).Property("FormName")?.Value;
            return formNameToken == null || formNameToken.Type == JTokenType.Null
                ? string.Empty
                : formNameToken.ToString();
        }
        catch
        {
            return string.Empty;
        }
    }

    private static void AppendEventOnlyExceptions(List<TableExceptionModel> merged, EventConfig eventData)
    {
        if (eventData?.TableExceptions == null)
        {
            return;
        }

        foreach (var ex in eventData.TableExceptions)
        {
            if (string.IsNullOrWhiteSpace(ex?.Field) || string.IsNullOrWhiteSpace(ex?.Table))
            {
                continue;
            }

            if (!EventOnlyTargetTables.Contains(ex.Table))
            {
                continue;
            }

            if (merged.Any(m =>
                    m.Field.Equals(ex.Field, System.StringComparison.OrdinalIgnoreCase)
                    && m.Table.Equals(ex.Table, System.StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            merged.Add(new TableExceptionModel { Field = ex.Field, Table = ex.Table });
        }
    }
}
