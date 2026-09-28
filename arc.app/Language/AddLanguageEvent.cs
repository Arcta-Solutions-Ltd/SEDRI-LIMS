using arc.app.Common;
using arc.app.Config.Queries;
using arc.app.Configuration;
using arc.common;
using arc.common.Models.Language;
using arc.domain.Configuration.EventsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace arc.app.Language
{
    public class AddLanguageEvent : IRun
    {
        private readonly IConfigFactory _configFactory;
        private readonly IQueryAdapter _queryAdapter;
        private readonly ILanguageRepository _languageRepository;

        public AddLanguageEvent(IConfigFactory configFactory, IQueryAdapter queryAdapter, ILanguageRepository languageRepository)
        {
            _configFactory = configFactory;
            _queryAdapter = queryAdapter;
            _languageRepository = languageRepository;
        }

        public async Task<int> RunAsync(string dataToSave, string Id, EventModel command, EventConfig eventData = null)
        {
            var record = JsonConvert.DeserializeObject<LanguageModel>(dataToSave);

            var queryParam = @"{ 'Name': 'LanguageList', 'Parameters': [ {'Key': 'TranslationId', 'Value': '" + record.SourceId + "'}] }";
            var queryFilters = JsonConvert.DeserializeObject<QueryFilterConfig>(queryParam);
            var queryData = await _queryAdapter.GetQueryAsync(queryFilters.Name);
            var configHandler = _configFactory.Create(queryFilters.Name);
            var languageList = await configHandler.GetAsync(queryFilters, queryData);

            var recordToSave = JsonConvert.DeserializeObject<LanguageModel>(dataToSave);
            recordToSave.Pack = languageList;

            var recordId = await _languageRepository.AddLanguageAsync(recordToSave);

            return recordId;
        }
    }
}
