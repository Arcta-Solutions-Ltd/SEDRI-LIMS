using arc.app.Common;
using arc.app.Configuration;
using arc.common.Models.Language;
using arc.domain.Configuration.LanguageConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Repository for managing language-related operations.
    /// </summary>
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ISqlCommand _sqlCommand;
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageRepository"/> class.
        /// </summary>
        /// <param name="sqlCommand">The SQL command executor.</param>
        /// <param name="sqlQuery">The SQL query executor.</param>
        /// <param name="logWriter">The log writer for logging information.</param>
        public LanguageRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlCommand = sqlCommand ?? throw new ArgumentNullException(nameof(sqlCommand));
            _sqlQuery = sqlQuery ?? throw new ArgumentNullException(nameof(sqlQuery));
            _logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Gets a list of languages asynchronously based on the specified query filter configuration.
        /// </summary>
        /// <param name="parameters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of language items.</returns>
        public async Task<List<LanguageItem>> GetLanguageAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get language query", nameof(LanguageRepository), nameof(GetLanguageAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new GetLanguageQuery(), "Get Language Query", parameters);
        }

        /// <summary>
        /// Gets a language by translation ID asynchronously based on the specified query filter configuration.
        /// </summary>
        /// <param name="parameters">The query filter configuration.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the language model.</returns>
        public async Task<LanguageModel> GetLanguageByTranslationIdAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Run get language by translation ID query", nameof(LanguageRepository), nameof(GetLanguageByTranslationIdAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new GetLanguageByTranslationIdQuery(), "Get Language By Translation ID Query", parameters);
        }

        /// <summary>
        /// Adds a language asynchronously.
        /// </summary>
        /// <param name="dataToSave">The language model to save.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added language.</returns>
        public async Task<int> AddLanguageAsync(LanguageModel dataToSave)
        {
            _logWriter.LogInfo("Run add language command", nameof(LanguageRepository), nameof(AddLanguageAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddLanguageCommand(), "Add Language", dataToSave);
        }

        /// <summary>
        /// Deletes a language asynchronously based on the specified translation ID.
        /// </summary>
        /// <param name="translationId">The translation ID of the language to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the translation ID of the deleted language.</returns>
        public async Task<int> DeleteLanguageAsync(int translationId)
        {
            _logWriter.LogInfo("Run delete language command", nameof(LanguageRepository), nameof(DeleteLanguageAsync));
            await _sqlCommand.CarryOutCommandAsync(new DeleteLanguageCommand(), "Delete Language", translationId.ToString());
            return translationId;
        }

        /// <summary>
        /// Updates a language pack asynchronously.
        /// </summary>
        /// <param name="dataToSave">The language model to update.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the updated language pack.</returns>
        public async Task<int> UpdateLanguagePackAsync(LanguageModel dataToSave)
        {
            _logWriter.LogInfo("Run update language pack command", nameof(LanguageRepository), nameof(UpdateLanguagePackAsync));
            return await _sqlCommand.CommandWithTypeQueryAsync(new UpdateLanguagePackCommand(), "Update Language Pack", dataToSave);
        }

        /// <summary>
        /// Gets all languages asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of all languages.</returns>
        public async Task<IEnumerable<AllLanguageModel>> AllLanguagesAsync()
        {
            _logWriter.LogInfo("Run get all languages query", nameof(LanguageRepository), nameof(AllLanguagesAsync));
            return await _sqlQuery.QueryReturningTypeAsync(new AllLanguagesQuery(), "Get the list of languages", new QueryFilterConfig());
        }
    }
}
