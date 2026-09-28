using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Diagnostics;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    /// <summary>
    /// Handles bulk direct test selection for a specimen (TestSelection event).
    /// </summary>
    public class TestSelectionEvent : IRun
    {
        private readonly ITestRepository _testRepository;
        private readonly ILogWriter _logWriter;

        public TestSelectionEvent(ITestRepository testRepository, ILogWriter logWriter)
        {
            _testRepository = testRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Adds selected direct tests to the specimen identified by <paramref name="id"/>.
        /// </summary>
        /// <param name="dataToSave">Crafted test selection payload.</param>
        /// <param name="id">Specimen id.</param>
        /// <param name="command">Workflow command metadata.</param>
        /// <param name="eventData">Optional event configuration.</param>
        /// <returns>The specimen id.</returns>
        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var stopwatch = Stopwatch.StartNew();
            _logWriter.LogInfo(
                $"TestSelection start specimenId={id}",
                nameof(TestSelectionEvent),
                nameof(RunAsync));

            await _testRepository.AddTestsAsync(dataToSave, id, command);

            stopwatch.Stop();
            _logWriter.LogInfo(
                $"TestSelection complete specimenId={id} elapsedMs={stopwatch.ElapsedMilliseconds}",
                nameof(TestSelectionEvent),
                nameof(RunAsync));

            return int.Parse(id);
        }
    }
}
