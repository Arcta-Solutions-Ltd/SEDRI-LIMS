using arc.app.Configuration;
using arc.domain.Configuration.LanguageConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Config.Language;

/// <summary>
/// Factory class for managing language-related operations.
/// </summary>
public class LanguageFactory : ILanguageFactory
{
    /// <summary>
    /// Repository for accessing language-related data.
    /// </summary>
    private readonly ILanguageRepository _languageRepository;

    /// <summary>
    /// Logger for logging language factory operations and errors.
    /// </summary>
    private readonly ILogger<LanguageFactory> _logger;

    /// <summary>
    /// Host environment for determining the current application environment.
    /// </summary>
    private readonly IHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the <see cref="LanguageFactory"/> class.
    /// </summary>
    /// <param name="languageRepository">The language repository used for retrieving language data.</param>
    /// <param name="logger">The logger for logging language factory operations and errors.</param>
    /// <param name="env">The host environment for determining the current application environment.</param>
    public LanguageFactory(ILanguageRepository languageRepository, ILogger<LanguageFactory> logger, IHostEnvironment env)
    {
        _languageRepository = languageRepository;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Retrieves a list of language items asynchronously based on the provided language ID.
    /// </summary>
    /// <param name="languageId">The unique identifier for the language to retrieve.</param>
    /// <returns>
    /// A <see cref="Task"/> that represents the asynchronous operation, containing a list of 
    /// <see cref="LanguageItem"/> objects for the specified language.
    /// </returns>
    public async Task<List<LanguageItem>> GetAsync(string languageId)
    {
        var parameters = new QueryFilterConfig
        {
            Parameters = [new QueryValuesConfig { Key = "translationid", Value = languageId }]
        };

        var language = await _languageRepository.GetLanguageAsync(parameters);

        if (language.Count == 0)
        {
            var defaultEnglish = new EnglishLanguage().Get();
            language = JsonConvert.DeserializeObject<List<LanguageItem>>(defaultEnglish);
        }

        var languageItems = language.ToList();

        var duplicateLanguageTags = GetDuplicateLanguageTags(languageItems);

        if (duplicateLanguageTags.Count > 0)
        {
            if (_env.IsDevelopment())
            {

                throw new Exception($"Duplicate language tags: {string.Join(", ", duplicateLanguageTags)}");
            }
            else
            {
                _logger.LogError("Duplicate language tags: {Tags}", string.Join(", ", duplicateLanguageTags));
            }
        }

        return languageItems;
    }

    /// <summary>
    /// Finds and returns a list of duplicate language tag keys.
    /// </summary>
    /// <param name="languageItems">The list of language items to check for duplicates.</param>
    /// <returns>A list of duplicate keys found in the language items.</returns>
    private static List<string> GetDuplicateLanguageTags(List<LanguageItem> languageItems)
    {
        return [.. languageItems
            .GroupBy(item => item.Key)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)];
    }
}
