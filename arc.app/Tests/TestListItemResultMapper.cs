using arc.app.SystemConfig;
using arc.common;
using arc.domain.Tests;
using Newtonsoft.Json;
using System;

namespace arc.app.Tests
{
    /// <summary>
    /// Maps a single serialized test list row: loads form layout via <see cref="ITestResultsForListFormatter"/> and updates description and formatted <c>TestResults</c>.
    /// </summary>
    public class TestListItemResultMapper : IMap
    {
        private readonly ITestResultsForListFormatter _testResultsForListFormatter;
        private readonly IConfigItemHandler _configItemHandler;

        public TestListItemResultMapper(ITestResultsForListFormatter testResultsForListFormatter, IConfigItemHandler configItemHandler)
        {
            _testResultsForListFormatter = testResultsForListFormatter;
            _configItemHandler = configItemHandler;
        }

        /// <summary>
        /// Deserializes a test row and applies list/callout formatting (field order from full form definition).
        /// </summary>
        /// <param name="source">JSON for one <see cref="TestList"/> row.</param>
        /// <returns>Serialized <see cref="TestList"/> with updated description and <c>TestResults</c>.</returns>
        public string Map(string source)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var testDetails = JsonConvert.DeserializeObject<TestList>(source, settings);

            var contents = _configItemHandler.GetSingleItemAsync(testDetails.TestName).Result;
            if (!string.IsNullOrEmpty(contents))
            {
                _testResultsForListFormatter.ApplyToTestAsync(testDetails, fieldOrderCache: null, applyYesNoUiAliases: false).GetAwaiter().GetResult();
            }

            return JsonConvert.SerializeObject(testDetails);
        }
    }
}
