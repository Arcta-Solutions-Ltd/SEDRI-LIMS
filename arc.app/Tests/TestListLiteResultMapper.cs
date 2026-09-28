using arc.app.SystemConfig;
using arc.common;
using arc.common.Models.SystemConfig;
using arc.common.Models.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace arc.app.Tests;

/// <summary>
/// Maps test list rows for embedded grid/card views: resolves display title only, without formatting TestResults.
/// </summary>
public class TestListLiteResultMapper : IMap
{
    private readonly IConfigItemHandler _configItemHandler;

    /// <summary>
    /// Creates a lite list mapper that loads form titles from configuration.
    /// </summary>
    /// <param name="configItemHandler">Resolves form config JSON by test name.</param>
    public TestListLiteResultMapper(IConfigItemHandler configItemHandler)
    {
        _configItemHandler = configItemHandler;
    }

    /// <summary>
    /// Parses a JSON array of test rows and sets <see cref="TestListResultModel.TestDescription"/> from config title.
    /// </summary>
    /// <param name="source">JSON array of test list rows.</param>
    /// <returns>JSON array with titles resolved; TestResults left unset for lazy callout loading.</returns>
    public string Map(string source)
    {
        JToken json;
        using (var sr = new StringReader(source))
        using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
        {
            json = JToken.ReadFrom(jr);
        }

        var mappedList = new List<TestListResultModel>();
        var titleCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };

        foreach (var child in json.Children())
        {
            var testDetails = JsonConvert.DeserializeObject<TestListResultModel>(child.ToString(), settings);
            if (testDetails == null || string.IsNullOrEmpty(testDetails.TestName))
            {
                continue;
            }

            testDetails.TestDescription = ResolveTitle(testDetails.TestName, titleCache);
            testDetails.TestResults = null;
            mappedList.Add(testDetails);
        }

        return JsonConvert.SerializeObject(mappedList);
    }

    private string ResolveTitle(string testName, Dictionary<string, string> titleCache)
    {
        if (titleCache.TryGetValue(testName, out var cachedTitle))
        {
            return cachedTitle;
        }

        var contents = _configItemHandler.GetSingleItemAsync(testName).GetAwaiter().GetResult();
        var title = string.Empty;
        if (!string.IsNullOrEmpty(contents))
        {
            try
            {
                title = JsonConvert.DeserializeObject<NameConfigModel>(contents)?.Title ?? string.Empty;
            }
            catch (JsonException)
            {
            }
        }

        titleCache[testName] = title;
        return title;
    }
}
