using arc.app.Common;
using arc.app.Configuration;
using arc.app.Import;
using arc.app.SystemConfig;
using arc.common.Models;
using arc.common.Models.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.PagesConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.ExpertRule;

/// <summary>
/// Resolves stored test config names, field ids, and list-type comp values to user-facing labels for the test condition embedded list.
/// Matching uses stable ids only; display text may vary by language.
/// </summary>
public class ExpertRuleTestConditionListDisplayEnricher : IExpertRuleTestConditionListDisplayEnricher
{
    private static readonly HashSet<string> ListFieldTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "combobox",
        "dropdown",
        "hierarchicalpicker"
    };

    private readonly IConfigListDataHandler _configListDataHandler;
    private readonly IFieldListHandler _fieldListHandler;
    private readonly IFormConfigDefinition _formConfigDefinition;
    private readonly ILanguageHandler _languageHandler;
    private readonly IListRepository _listRepository;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpertRuleTestConditionListDisplayEnricher"/> class.
    /// </summary>
    /// <param name="configListDataHandler">Provides test config name to title mapping.</param>
    /// <param name="fieldListHandler">Provides field id to label mapping per test form.</param>
    /// <param name="formConfigDefinition">Provides test form field metadata for comp value resolution.</param>
    /// <param name="languageHandler">Translates config title keys to user-facing text.</param>
    /// <param name="listRepository">Resolves list item ids to display values.</param>
    /// <param name="logWriter">Logger for installed-system diagnostics.</param>
    public ExpertRuleTestConditionListDisplayEnricher(
        IConfigListDataHandler configListDataHandler,
        IFieldListHandler fieldListHandler,
        IFormConfigDefinition formConfigDefinition,
        ILanguageHandler languageHandler,
        IListRepository listRepository,
        ILogWriter logWriter)
    {
        _configListDataHandler = configListDataHandler;
        _fieldListHandler = fieldListHandler;
        _formConfigDefinition = formConfigDefinition;
        _languageHandler = languageHandler;
        _listRepository = listRepository;
        _logWriter = logWriter;
    }

    /// <inheritdoc />
    public async Task<List<ExpertRuleTestConditionListModel>> EnrichAsync(
        IReadOnlyList<ExpertRuleTestConditionListModel> rows,
        TokenInfoModel token)
    {
        if (rows == null || rows.Count == 0)
        {
            return [];
        }

        var testOptions = await _configListDataHandler.GetTestListAsync();
        var testTitleByKey = BuildOptionLookup(testOptions);
        var fieldCache = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        var formFieldCache = new Dictionary<string, Dictionary<string, FieldConfig>>(StringComparer.OrdinalIgnoreCase);
        var listValueCache = new Dictionary<int, string>();

        var result = new List<ExpertRuleTestConditionListModel>(rows.Count);
        foreach (var row in rows)
        {
            var enriched = new ExpertRuleTestConditionListModel
            {
                Id = row.Id,
                Comparison = row.Comparison,
                CompValue = await ResolveCompValueDisplayAsync(row, formFieldCache, listValueCache),
                TestName = await ResolveTestTitleAsync(row, testTitleByKey, token),
                FieldName = await ResolveFieldLabelAsync(row, token, fieldCache)
            };
            result.Add(enriched);
        }

        return result;
    }

    /// <summary>
    /// Resolves a stored comp value id to list item display text when the target field is list-backed.
    /// </summary>
    /// <param name="row">List row with stored test form id, field id, and comp value.</param>
    /// <param name="formFieldCache">Cache of field configs keyed by test form name.</param>
    /// <param name="listValueCache">Cache of resolved list item id to display text.</param>
    /// <returns>Display text for list fields; unchanged comp value for other field types.</returns>
    private async Task<string> ResolveCompValueDisplayAsync(
        ExpertRuleTestConditionListModel row,
        Dictionary<string, Dictionary<string, FieldConfig>> formFieldCache,
        Dictionary<int, string> listValueCache)
    {
        var compValue = row.CompValue;
        if (string.IsNullOrWhiteSpace(compValue))
        {
            return compValue;
        }

        var storedTestName = row.TestName;
        var storedFieldName = row.FieldName;
        if (string.IsNullOrWhiteSpace(storedTestName) || string.IsNullOrWhiteSpace(storedFieldName))
        {
            return compValue;
        }

        var fieldConfig = await GetFieldConfigAsync(storedTestName, storedFieldName, formFieldCache);
        if (fieldConfig == null || !IsListBackedField(fieldConfig))
        {
            return compValue;
        }

        if (!int.TryParse(compValue.Trim(), out var listItemId) || listItemId <= 0)
        {
            return compValue;
        }

        if (listValueCache.TryGetValue(listItemId, out var cachedValue))
        {
            return cachedValue;
        }

        var resolved = await _listRepository.GetValueFromIdAsync(listItemId);
        if (string.IsNullOrWhiteSpace(resolved))
        {
            _logWriter.LogInfo(
                $"Expert rule test condition list: unresolved comp value listItemId={listItemId} field={storedFieldName} test={storedTestName} testConditionId={row.Id}",
                nameof(ExpertRuleTestConditionListDisplayEnricher),
                nameof(ResolveCompValueDisplayAsync));
            return compValue;
        }

        listValueCache[listItemId] = resolved;
        return resolved;
    }

    /// <summary>
    /// Loads field configuration for a test form field id from the form definition cache.
    /// </summary>
    /// <param name="testFormName">Stored test form config name stored on the row.</param>
    /// <param name="fieldId">Stored field id stored on the row.</param>
    /// <param name="formFieldCache">Per-form field lookup cache.</param>
    /// <returns>Field config when found; otherwise null.</returns>
    private async Task<FieldConfig> GetFieldConfigAsync(
        string testFormName,
        string fieldId,
        Dictionary<string, Dictionary<string, FieldConfig>> formFieldCache)
    {
        if (!formFieldCache.TryGetValue(testFormName, out var fieldLookup))
        {
            var formConfig = await _formConfigDefinition.LoadFormAsync(testFormName);
            var fields = formConfig?.GetFieldsForForm() ?? [];
            fieldLookup = fields
                .Where(f => !string.IsNullOrWhiteSpace(f?.Id))
                .GroupBy(f => f.Id, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
            formFieldCache[testFormName] = fieldLookup;
        }

        fieldLookup.TryGetValue(fieldId, out var fieldConfig);
        return fieldConfig;
    }

    /// <summary>
    /// Returns true when the field type resolves list item ids for comp value display.
    /// </summary>
    /// <param name="fieldConfig">Test form field configuration.</param>
    /// <returns>True for combobox, dropdown, and hierarchical picker fields.</returns>
    private static bool IsListBackedField(FieldConfig fieldConfig) =>
        fieldConfig != null &&
        !string.IsNullOrWhiteSpace(fieldConfig.Type) &&
        ListFieldTypes.Contains(fieldConfig.Type);

    private async Task<string> ResolveTestTitleAsync(
        ExpertRuleTestConditionListModel row,
        IReadOnlyDictionary<string, string> testTitleByKey,
        TokenInfoModel token)
    {
        var storedTestName = row.TestName;
        if (string.IsNullOrWhiteSpace(storedTestName))
        {
            return storedTestName;
        }

        if (testTitleByKey.TryGetValue(storedTestName, out var title))
        {
            return await _languageHandler.TranslateAsync(title, token.LanguageId);
        }

        _logWriter.LogInfo(
            $"Expert rule test condition list: unresolved test config id={storedTestName} testConditionId={row.Id}",
            nameof(ExpertRuleTestConditionListDisplayEnricher),
            nameof(ResolveTestTitleAsync));

        return storedTestName;
    }

    private async Task<string> ResolveFieldLabelAsync(
        ExpertRuleTestConditionListModel row,
        TokenInfoModel token,
        Dictionary<string, Dictionary<string, string>> fieldCache)
    {
        var storedFieldName = row.FieldName;
        var storedTestName = row.TestName;
        if (string.IsNullOrWhiteSpace(storedFieldName))
        {
            return storedFieldName;
        }

        if (string.IsNullOrWhiteSpace(storedTestName))
        {
            return storedFieldName;
        }

        if (!fieldCache.TryGetValue(storedTestName, out var fieldLabels))
        {
            var fields = await _fieldListHandler.GetFieldsAsync(token, storedTestName);
            fieldLabels = fields?
                .Where(f => !string.IsNullOrWhiteSpace(f?.Key))
                .GroupBy(f => f.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Text, StringComparer.OrdinalIgnoreCase)
                ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            fieldCache[storedTestName] = fieldLabels;
        }

        if (fieldLabels.TryGetValue(storedFieldName, out var label))
        {
            return label;
        }

        _logWriter.LogInfo(
            $"Expert rule test condition list: unresolved field id={storedFieldName} test={storedTestName} testConditionId={row.Id}",
            nameof(ExpertRuleTestConditionListDisplayEnricher),
            nameof(ResolveFieldLabelAsync));

        return storedFieldName;
    }

    private static Dictionary<string, string> BuildOptionLookup(IReadOnlyList<OptionsConfig> options)
    {
        if (options == null || options.Count == 0)
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        return options
            .Where(o => !string.IsNullOrWhiteSpace(o?.Key))
            .GroupBy(o => o.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Text ?? g.Key, StringComparer.OrdinalIgnoreCase);
    }
}
