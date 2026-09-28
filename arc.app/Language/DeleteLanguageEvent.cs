using arc.app.Common;
using arc.app.Configuration;
using arc.common;
using arc.common.Models.Language;
using arc.domain.Configuration.EventsConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Language
{
    public class DeleteLanguageEvent : IRun
    {
        private readonly ILanguageRepository _languageRepository;

        public DeleteLanguageEvent(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var record = JsonConvert.DeserializeObject<LanguageModel>(dataToSave);
            var recordId = await _languageRepository.DeleteLanguageAsync(int.Parse(Id));

            return recordId;
        }
    }
}
