using arc.app.Common;
using arc.common;
using arc.domain.Configuration.EventsConfig;
using System.Threading.Tasks;

namespace arc.app.Tests
{
    public class CultureTestSelectionEvent : IRun
    {
        public ITestRepository _testRepository;

        public CultureTestSelectionEvent(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            await _testRepository.AddCultureTestsAsync(dataToSave, id);
            return int.Parse(id);
        }
    }
}
