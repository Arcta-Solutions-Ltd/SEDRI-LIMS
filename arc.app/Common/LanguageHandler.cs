using arc.app.Config.Language;
using arc.common;
using arc.common.Utils;
using arc.domain.Configuration.LanguageConfig;
using arc.domain.Configuration.ListsConfig;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arc.app.Common;

/// <summary>
/// Handles language translation operations.
/// </summary>
public class LanguageHandler : ILanguageHandler
{
    private readonly ILanguageFactory _languageFactory;
    private readonly IMapObjectArrayToJson _jsonMapper;
    private readonly ICacheManager _cacheManager;
    private readonly ILogger<LanguageHandler> _logger;
    private readonly IHostEnvironment _env;

    /// <summary>
    /// Initializes a new instance of the LanguageHandler class.
    /// </summary>
    /// <param name="languageFactory">The language factory instance.</param>
    /// <param name="jsonMapper">The JSON mapper instance.</param>
    /// <param name="cacheManager">The cache manager instance.</param>
    public LanguageHandler(ILanguageFactory languageFactory, IMapObjectArrayToJson jsonMapper, ICacheManager cacheManager, ILogger<LanguageHandler> logger, IHostEnvironment env)
    {
        _languageFactory = languageFactory;
        _jsonMapper = jsonMapper;
        _cacheManager = cacheManager;
        _logger = logger;
        _env = env;
    }

    /// <summary>
    /// Asynchronously translates the given text to the specified language.
    /// </summary>
    /// <param name="textToTranslate">The text to translate.</param>
    /// <param name="languageId">The ID of the target language.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated text.</returns>
    public async Task<string> TranslateAsync(string textToTranslate, string languageId)
    {
        var language = await GetLanguageItemsFromCacheAsync(languageId);

        if (string.IsNullOrEmpty(textToTranslate) || language == null || language.Count == 0)
        {
            return textToTranslate;
        }

        // Tags are '@...@'-delimited tokens. A single left-to-right pass with a token
        // lookup replaces every token in one scan (O(text)), instead of running a full
        // string.Replace for each of the ~2,400 tags (O(tags * text)). Output is identical:
        // unknown tokens are left untouched and lookups stay case-insensitive.
        var lookup = BuildTokenLookup(language);

        var result = new StringBuilder(textToTranslate.Length);
        var index = 0;
        var length = textToTranslate.Length;

        while (index < length)
        {
            var current = textToTranslate[index];
            if (current == '@')
            {
                var closing = textToTranslate.IndexOf('@', index + 1);
                if (closing > index)
                {
                    var token = textToTranslate.Substring(index, closing - index + 1);
                    if (lookup.TryGetValue(token, out var value))
                    {
                        result.Append(value);
                        index = closing + 1;
                        continue;
                    }
                }
            }

            result.Append(current);
            index++;
        }

        return result.ToString();
    }

    /// <summary>
    /// Translates @...@ tokens in JSON string values without breaking JSON structure.
    /// Walks the token tree and re-serializes with proper escaping for translated values.
    /// </summary>
    /// <param name="json">JSON string whose string values may contain language tokens.</param>
    /// <param name="languageId">The ID of the target language.</param>
    /// <returns>JSON with translated string values, or the original input when empty or not parseable as JSON.</returns>
    public async Task<string> TranslateJsonAsync(string json, string languageId)
    {
        if (string.IsNullOrEmpty(json))
        {
            return json;
        }

        JToken root;
        try
        {
            root = JToken.Parse(json);
        }
        catch (JsonReaderException)
        {
            return await TranslateAsync(json, languageId);
        }

        var language = await GetLanguageItemsFromCacheAsync(languageId);
        if (language == null || language.Count == 0)
        {
            return json;
        }

        var lookup = BuildTokenLookup(language);

        if (root.Type == JTokenType.String)
        {
            var rootText = root.Value<string>();
            if (string.IsNullOrEmpty(rootText) || rootText.IndexOf('@') < 0)
            {
                return json;
            }

            var translatedRoot = TranslateTokenString(rootText, lookup);
            return JValue.CreateString(translatedRoot).ToString(Formatting.None);
        }

        try
        {
            TranslateJsonTokenValues(root, lookup);
            return root.ToString(Formatting.None);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "TranslateJsonAsync failed after parsing JSON (length={JsonLength}, languageId={LanguageId})",
                json.Length,
                languageId);
            throw;
        }
    }

    /// <summary>
    /// Recursively translates @...@ tokens in string leaf values of a JSON token tree.
    /// String leaves are updated through their parent <see cref="JProperty"/> or <see cref="JArray"/>
    /// so re-serialization applies correct JSON escaping without using <see cref="JToken.Replace"/>.
    /// </summary>
    /// <param name="token">The JSON token to translate in place.</param>
    /// <param name="lookup">Case-insensitive token-to-value lookup.</param>
    private void TranslateJsonTokenValues(JToken token, Dictionary<string, string> lookup)
    {
        if (token is JObject obj)
        {
            foreach (var property in obj.Properties().ToList())
            {
                if (property.Value?.Type == JTokenType.String)
                {
                    AssignTranslatedString(property, lookup);
                }
                else if (property.Value != null)
                {
                    TranslateJsonTokenValues(property.Value, lookup);
                }
            }

            return;
        }

        if (token is JArray array)
        {
            for (var i = 0; i < array.Count; i++)
            {
                var item = array[i];
                if (item?.Type == JTokenType.String)
                {
                    AssignTranslatedString(array, i, lookup);
                }
                else if (item != null)
                {
                    TranslateJsonTokenValues(item, lookup);
                }
            }
        }
    }

    /// <summary>
    /// Translates @...@ tokens in a JSON object property when the property value is a string.
    /// </summary>
    /// <param name="property">The object property whose string value may contain language tokens.</param>
    /// <param name="lookup">Case-insensitive token-to-value lookup.</param>
    private void AssignTranslatedString(JProperty property, Dictionary<string, string> lookup)
    {
        var text = property.Value?.Value<string>();
        if (string.IsNullOrEmpty(text) || text.IndexOf('@') < 0)
        {
            return;
        }

        var translated = TranslateTokenString(text, lookup);
        if (!string.Equals(text, translated, StringComparison.Ordinal))
        {
            if (_env.IsDevelopment())
            {
                _logger.LogDebug(
                    "TranslateJsonAsync translated string at path {TokenPath}",
                    property.Path);
            }

            property.Value = translated;
        }
    }

    /// <summary>
    /// Translates @...@ tokens in a string element of a JSON array.
    /// </summary>
    /// <param name="array">The array containing the string element.</param>
    /// <param name="index">The index of the string element.</param>
    /// <param name="lookup">Case-insensitive token-to-value lookup.</param>
    private void AssignTranslatedString(JArray array, int index, Dictionary<string, string> lookup)
    {
        var text = array[index].Value<string>();
        if (string.IsNullOrEmpty(text) || text.IndexOf('@') < 0)
        {
            return;
        }

        var translated = TranslateTokenString(text, lookup);
        if (string.Equals(text, translated, StringComparison.Ordinal))
        {
            return;
        }

        if (_env.IsDevelopment())
        {
            _logger.LogDebug(
                "TranslateJsonAsync translated string at path {TokenPath}",
                array[index].Path);
        }

        array[index] = translated;
    }

    /// <summary>
    /// Replaces @...@ tokens in a single string using the language lookup.
    /// </summary>
    /// <param name="textToTranslate">Text that may contain language tokens.</param>
    /// <param name="lookup">Case-insensitive token-to-value lookup.</param>
    /// <returns>Translated text with unknown tokens left unchanged.</returns>
    private static string TranslateTokenString(string textToTranslate, Dictionary<string, string> lookup)
    {
        var result = new StringBuilder(textToTranslate.Length);
        var index = 0;
        var length = textToTranslate.Length;

        while (index < length)
        {
            var current = textToTranslate[index];
            if (current == '@')
            {
                var closing = textToTranslate.IndexOf('@', index + 1);
                if (closing > index)
                {
                    var token = textToTranslate.Substring(index, closing - index + 1);
                    if (lookup.TryGetValue(token, out var value))
                    {
                        result.Append(value);
                        index = closing + 1;
                        continue;
                    }
                }
            }

            result.Append(current);
            index++;
        }

        return result.ToString();
    }

    /// <summary>
    /// Builds a case-insensitive token-to-value lookup from the language pack. Where the same
    /// token appears more than once, the first entry wins, matching the behaviour of the previous
    /// sequential <see cref="string.Replace(string, string, StringComparison)"/> loop.
    /// </summary>
    /// <param name="language">The language items for the requested language.</param>
    /// <returns>A dictionary mapping each '@...@' token to its translated value.</returns>
    private static Dictionary<string, string> BuildTokenLookup(List<LanguageItem> language)
    {
        var lookup = new Dictionary<string, string>(language.Count, StringComparer.OrdinalIgnoreCase);

        foreach (var item in language)
        {
            if (item?.Key == null || lookup.ContainsKey(item.Key))
            {
                continue;
            }

            lookup[item.Key] = item.Value ?? string.Empty;
        }

        return lookup;
    }

    /// <summary>
    /// Asynchronously translates a list of configurations to the specified language.
    /// </summary>
    /// <param name="listToTranslate">The list of configurations to translate.</param>
    /// <param name="languageId">The ID of the target language.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the translated list of configurations.</returns>
    public async Task<List<ListConfig>> TranslateListAsync(List<ListConfig> listToTranslate, string languageId)
    {
        var language = await GetLanguageItemsFromCacheAsync(languageId);

        foreach (var entry in listToTranslate)
        {
            var optionsList = new List<OptionsConfig>();
            foreach (var option in entry.Options)
            {
                if (option.Text.Contains('@'))
                {
                    var languageEntry = language.First(l => l.Key == option.Text);
                    if (languageEntry != null)
                    {
                        option.Text = languageEntry.Value;
                    }
                }
                optionsList.Add(option);
            }
            entry.Options = optionsList;
        }

        return listToTranslate;
    }

    /// <summary>
    /// Asynchronously gets the front-end tags for the specified language.
    /// </summary>
    /// <param name="languageId">The ID of the target language.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the front-end tags as a JSON string.</returns>
    public async Task<string> GetFrontEndTagsAsync(string languageId)
    {
        var language = await GetLanguageItemsFromCacheAsync(languageId);
        var frontendLanguageItems = new List<LanguageItem>();
        var missingLanguageItemTags = new List<string>();

        foreach (var key in FrontEndTags.Get())
        {
            if (TryGetByKey(language, key, out var value))
            {
                frontendLanguageItems.Add(value);
            }
            else
            {
                missingLanguageItemTags.Add(key);
            }
        }

        if (missingLanguageItemTags.Count > 0)
        {
            if (_env.IsDevelopment())
            {
                throw new Exception($"Missing language tags: {string.Join(", ", missingLanguageItemTags)}");
            }
            else
            {
                _logger.LogError("Missing language tags: {Tags}", string.Join(", ", missingLanguageItemTags));
                foreach (var missingTag in missingLanguageItemTags)
                {
                    frontendLanguageItems.Add(new LanguageItem { Key = missingTag, Value = missingTag });
                }
            }
        }

        return _jsonMapper.Map(frontendLanguageItems);
    }

    /// <summary>
    /// Asynchronously gets the language items from the cache.
    /// </summary>
    /// <param name="languageId">The ID of the target language.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of language items.</returns>
    private async ValueTask<List<LanguageItem>> GetLanguageItemsFromCacheAsync(string languageId)
    {
        return await _cacheManager.GetAsync(async () => await _languageFactory.GetAsync(languageId), languageId);
    }

    /// <summary>
    /// Attempts to find a <see cref="LanguageItem"/> in the collection with the specified key.
    /// </summary>
    /// <param name="items">The collection of <see cref="LanguageItem"/> key value pairs to search.</param>
    /// <param name="key">The key to locate in the collection.</param>
    /// <param name="result">
    /// When this method returns, contains the <see cref="LanguageItem"/> with the specified key,
    /// if found; otherwise, <c>null</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if a <see cref="LanguageItem"/> with the specified key is found; otherwise, <c>false</c>.
    /// </returns>
    private static bool TryGetByKey(IEnumerable<LanguageItem> items, string key, out LanguageItem result)
    {
        result = items.FirstOrDefault(l => string.Equals(l.Key, key, StringComparison.OrdinalIgnoreCase));
        return result != null;
    }
}
