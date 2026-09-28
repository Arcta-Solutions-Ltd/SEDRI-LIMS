using arc.app.Common;
using arc.common;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using arc.app.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using arc.common.Models.Language;
using System.Linq;
using arc.domain.Configuration.EventsConfig;

namespace arc.app.Language
{
    internal class EditWordEvent : IRun
    {
        private readonly IServiceProvider _serviceProvider;

        public EditWordEvent(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> RunAsync(string dataToSave, string id, EventModel command, EventConfig eventData = null)
        {
            var languageRepository = _serviceProvider.GetService<ILanguageRepository>();

            var model = JsonConvert.DeserializeObject<LanguageEditModel>(dataToSave);
            var translationId = int.Parse(model.MetafTranslationId);

            var queryFilter = new QueryFilterConfig();
            queryFilter.AddInteger("TranslationId", translationId);

            var languageList = await languageRepository.GetLanguageAsync(queryFilter);

            var languageItem = languageList.Where((l) => l.Key == model.Key).FirstOrDefault();
            languageItem.Value = model.Value;

            var pack = JsonConvert.SerializeObject(languageList);
            await languageRepository.UpdateLanguagePackAsync(new LanguageModel { TranslationId = translationId, Pack = pack });

            return 0;
        }
    }
}
