using arc.app.Common;
using arc.app.Config.Language;
using arc.app.Configuration;
using arc.common.Models.Language;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Language
{
    /// <summary>
    /// Handles the operations related to language lists.
    /// </summary>
    public class LanguageListHandler : ILanguageListHandler
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ILanguageFactory _languageFactory;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageListHandler"/> class.
        /// </summary>
        /// <param name="languageRepository">The language repository.</param>
        /// <param name="languageFactory">The language factory.</param>
        /// <param name="logWriter">The log writer for logging information.</param>
        public LanguageListHandler(ILanguageRepository languageRepository, ILanguageFactory languageFactory, ILogWriter logWriter)
        {
            _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
            _languageFactory = languageFactory ?? throw new ArgumentNullException(nameof(languageFactory));
            _logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Gets the list of languages asynchronously based on the specified query filter configuration.
        /// </summary>
        /// <param name="parameters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of language models.</returns>
        public async Task<List<LanguageListModel>> GetListAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Get language by translationid", "LanguageListHandler", "GetList");
            var languageRecord = await _languageRepository.GetLanguageByTranslationIdAsync(parameters);

            _logWriter.LogInfo("Get language", "LanguageListHandler", "GetList");
            var languageList = await _languageRepository.GetLanguageAsync(parameters);

            var queryFilter = new QueryFilterConfig().AddInteger("TranslationId", languageRecord?.SourceId ?? 0);

            _logWriter.LogInfo("Get the original language list", "LanguageListHandler", "GetList");
            var originalLanguageList = await _languageRepository.GetLanguageAsync(queryFilter);

            if (originalLanguageList.Count == 0)
            {
                _logWriter.LogInfo("Get default language", "LanguageListHandler", "GetList");
                originalLanguageList = await _languageFactory.GetAsync("669");
            }

            var returnList = new List<LanguageListModel>();
            foreach (var item in languageList)
            {
                var original = originalLanguageList.FirstOrDefault(l => l.Key == item.Key);
                returnList.Add(new LanguageListModel
                {
                    Key = item.Key,
                    Value = item.Value,
                    Source = original?.Value ?? string.Empty
                });
            }

            return returnList;
        }
    }
}
