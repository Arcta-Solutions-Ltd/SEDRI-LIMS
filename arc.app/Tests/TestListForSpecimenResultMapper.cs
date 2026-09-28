using arc.common;
using arc.app.SystemConfig;
using arc.domain.Tests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System;

namespace arc.app.Tests
{
    /// <summary>
    /// Maps many serialized test rows for a specimen or culture: applies <see cref="ITestResultsForListFormatter"/> per row with a per-map field-order cache.
    /// </summary>
    /// <typeparam name="T">Concrete test model (direct or culture).</typeparam>
    public class TestListForSpecimenResultMapper<T> : IMap where T : Test
    {
        private readonly ITestResultsForListFormatter _testResultsForListFormatter;
        private readonly IConfigItemHandler _configItemHandler;

        public TestListForSpecimenResultMapper(ITestResultsForListFormatter testResultsForListFormatter, IConfigItemHandler configItemHandler)
        {
            _testResultsForListFormatter = testResultsForListFormatter;
            _configItemHandler = configItemHandler;
        }

        /// <summary>
        /// Parses a JSON array of tests and formats each row for display/callout order.
        /// </summary>
        /// <param name="source">JSON array of test records.</param>
        /// <returns>JSON array of mapped tests.</returns>
        public string Map(string source)
        {
            JToken json;
            using (var sr = new StringReader(source))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                json = JToken.ReadFrom(jr);
            }

            var mappedList = new List<Test>();
            var fieldOrderCache = new Dictionary<string, FormFieldOrderCache>();
            var configContentsCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var child in json.Children())
            {
                var record = child.ToString();
                var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
                var testDetails = JsonConvert.DeserializeObject<T>(record, settings);

                var contents = GetCachedConfigContents(testDetails.TestName, configContentsCache);
                if (string.IsNullOrEmpty(contents))
                    continue;

                _testResultsForListFormatter.ApplyToTestAsync(testDetails, fieldOrderCache, applyYesNoUiAliases: true).GetAwaiter().GetResult();

                mappedList.Add(testDetails);
            }

            return JsonConvert.SerializeObject(mappedList);
        }

        /// <summary>
        /// Returns config contents for a test form name, using a per-map cache to avoid repeated async lookups.
        /// </summary>
        /// <param name="testName">Stable test config name (Tests.testname).</param>
        /// <param name="cache">Cache populated during the current Map call.</param>
        /// <returns>Config JSON contents, or empty when not found.</returns>
        private string GetCachedConfigContents(string testName, Dictionary<string, string> cache)
        {
            if (string.IsNullOrEmpty(testName))
            {
                return string.Empty;
            }

            if (cache.TryGetValue(testName, out var cached))
            {
                return cached;
            }

            var contents = _configItemHandler.GetSingleItemAsync(testName).Result ?? string.Empty;
            cache[testName] = contents;
            return contents;
        }
    }
}
