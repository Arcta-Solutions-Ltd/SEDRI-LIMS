using arc.app.Common;
using arc.app.Config.Events;
using arc.app.Configuration;
using arc.app.SystemConfig;
using arc.common.ExtensionMethods;
using arc.common.Models.SystemConfig;
using arc.common.Models.Tests;
using arc.domain.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Tests;

/// <summary>
/// Implements test result formatting for list/callout views using hydrated form configs and save-event display rules.
/// </summary>
public class TestResultsForListFormatter : ITestResultsForListFormatter
{
    private readonly IFormConfigDefinition _formConfigDefinition;
    private readonly IJsonDataFormatter _jsonDataFormatter;
    private readonly IEventAdapter _eventAdapter;
    private readonly IConfigItemHandler _configItemHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestResultsForListFormatter"/> class.
    /// </summary>
    /// <param name="formConfigDefinition">Resolves full <see cref="FullFormConfig"/> including <c>PagesConfig</c> for field order.</param>
    /// <param name="jsonDataFormatter">Translates stored JSON to display-oriented structures.</param>
    /// <param name="eventAdapter">Loads save-event display metadata for the test.</param>
    /// <param name="configItemHandler">Fallback for form title when load fails.</param>
    public TestResultsForListFormatter(
        IFormConfigDefinition formConfigDefinition,
        IJsonDataFormatter jsonDataFormatter,
        IEventAdapter eventAdapter,
        IConfigItemHandler configItemHandler)
    {
        _formConfigDefinition = formConfigDefinition;
        _jsonDataFormatter = jsonDataFormatter;
        _eventAdapter = eventAdapter;
        _configItemHandler = configItemHandler;
    }

    /// <inheritdoc />
    public async Task ApplyToTestAsync(Test row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases)
    {
        if (row == null || string.IsNullOrEmpty(row.TestName))
            return;

        var results = row.TestResults;
        ApplyYesNoAliasesIfNeeded(ref results, applyYesNoUiAliases);

        var description = row.TestDescription;
        var formatted = await ApplyCoreAsync(row.TestName, description, results, fieldOrderCache);
        row.TestDescription = formatted.Description;
        row.TestResults = formatted.Results;
    }

    /// <inheritdoc />
    public async Task ApplyToListResultRowAsync(TestListResultModel row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases)
    {
        if (row == null || string.IsNullOrEmpty(row.TestName))
            return;

        var results = row.TestResults;
        ApplyYesNoAliasesIfNeeded(ref results, applyYesNoUiAliases);

        var description = row.TestDescription;
        var formatted = await ApplyCoreAsync(row.TestName, description, results, fieldOrderCache);
        row.TestDescription = formatted.Description;
        row.TestResults = formatted.Results;
    }

    /// <inheritdoc />
    public async Task<string> FormatCultureIsolateTestAsync(CultureTest row, Dictionary<string, FormFieldOrderCache> fieldOrderCache, bool applyYesNoUiAliases)
    {
        if (row == null || string.IsNullOrEmpty(row.TestName))
            return null;

        var results = row.TestResults;
        ApplyYesNoAliasesIfNeeded(ref results, applyYesNoUiAliases);

        var formatted = await ApplyCoreAsync(row.TestName, testDescription: null, results, fieldOrderCache);
        row.TestResults = formatted.Results;
        return formatted.Description ?? row.TestName;
    }

    private static void ApplyYesNoAliasesIfNeeded(ref string testResults, bool apply)
    {
        if (!apply || string.IsNullOrEmpty(testResults))
            return;

        if (testResults.Contains("Yes", StringComparison.Ordinal))
            testResults = testResults.Replace("Yes", "@GenYesA@", StringComparison.Ordinal);
        if (testResults.Contains("No", StringComparison.Ordinal))
            testResults = testResults.Replace("No", "@GenNo@", StringComparison.Ordinal);
    }

    private async Task<(string Description, string Results)> ApplyCoreAsync(
        string testName,
        string testDescription,
        string testResults,
        Dictionary<string, FormFieldOrderCache> fieldOrderCache)
    {
        var (order, titleFromForm) = await GetOrLoadFieldOrderAsync(testName, fieldOrderCache);

        if (!string.IsNullOrEmpty(titleFromForm))
            testDescription = titleFromForm;
        else if (string.IsNullOrEmpty(testDescription))
        {
            var contents = await _configItemHandler.GetSingleItemAsync(testName);
            if (!string.IsNullOrEmpty(contents))
            {
                try
                {
                    testDescription = JsonConvert.DeserializeObject<NameConfigModel>(contents)?.Title;
                }
                catch (JsonException)
                {
                }
            }
        }

        var eventName = GetEventNameForSaveEvent(testName);
        var eventDetails = await _eventAdapter.GetEventAsync(eventName);

        if (testResults != null && order != null && order.Count > 0)
        {
            try
            {
                var resultsToken = JToken.Parse(testResults);
                if (resultsToken.Type == JTokenType.Object)
                    testResults = testResults.ReorderPropertiesByKeyOrder(order);
            }
            catch (JsonReaderException)
            {
            }
        }

        if (testResults != null)
            testResults = await _jsonDataFormatter.TranslateAsync(testResults, eventDetails, order);

        return (testDescription, testResults);
    }

    private async Task<(IReadOnlyList<string> FieldIds, string Title)> GetOrLoadFieldOrderAsync(
        string testName,
        Dictionary<string, FormFieldOrderCache> fieldOrderCache)
    {
        if (fieldOrderCache != null && fieldOrderCache.TryGetValue(testName, out var cached))
        {
            return (cached?.FieldIds, cached?.Title);
        }

        FormFieldOrderCache entry;
        try
        {
            // Same key as IConfigItemHandler / FormConfigAdapter (e.g. DB Tests.TestName, typically *form).
            var loaded = await _formConfigDefinition.LoadFormAsync(testName);
            var ids = loaded?.GetOrderedFieldIdsForResults();
            entry = new FormFieldOrderCache
            {
                FieldIds = ids ?? Array.Empty<string>(),
                Title = loaded?.Title,
            };
        }
        catch (Exception)
        {
            entry = new FormFieldOrderCache { FieldIds = Array.Empty<string>(), Title = null };
        }

        fieldOrderCache?.Add(testName, entry);
        return (entry.FieldIds, entry.Title);
    }

    /// <summary>
    /// Maps a stored <paramref name="test"/> / form config name to the save-event name used for display configuration.
    /// </summary>
    /// <param name="test">Value from <c>Tests.TestName</c> / <c>CultureTests.TestName</c> (typically ends with <c>form</c>).</param>
    /// <returns>Event name for <see cref="IEventAdapter.GetEventAsync"/>.</returns>
    internal static string GetEventNameForSaveEvent(string test)
    {
        var returnTest = test.Replace(" ", "", StringComparison.Ordinal);

        if (returnTest.EndsWith("form", StringComparison.OrdinalIgnoreCase))
            returnTest = returnTest[..returnTest.LastIndexOf("form", StringComparison.OrdinalIgnoreCase)];

        return returnTest.ToLowerInvariant() switch
        {
            "directmicroscopy" => "MicroscopyTest",
            "fungalwetpreptest" => "KohPrepTest",
            "oxidasetest" => "OxidaseTest",
            "gramculture" => "GramCultureTest",
            _ => returnTest,
        };
    }
}
