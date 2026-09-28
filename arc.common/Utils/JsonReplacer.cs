using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace arc.common.Utils
{
    /// <summary>
    /// Provides functionality to modify JSON strings.
    /// </summary>
    public class JsonReplacer : IJsonReplacer
    {
        private IJsonElementRemover _jsonElementRemover;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonReplacer"/> class.
        /// </summary>
        /// <param name="jsonElementRemover">The JSON element remover service.</param>
        public JsonReplacer(IJsonElementRemover jsonElementRemover)
        {
            _jsonElementRemover = jsonElementRemover;
        }

        /// <summary>
        /// Changes the value of a specified key in a JSON string.
        /// Matching is case-insensitive at the root object: any existing properties whose names differ only by casing
        /// are removed, then a single property named <paramref name="key"/> (exact casing) is set.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="key">The key whose value needs to be changed.</param>
        /// <param name="newValue">The new value to set for the specified key.</param>
        /// <returns>The modified JSON string.</returns>
        public string ChangeValueInJsonString(string json, string key, string newValue)
        {
            var jsonObject = JObject.Parse(json);
            foreach (var prop in jsonObject.Properties().Where(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase)).ToList())
            {
                prop.Remove();
            }

            jsonObject[key] = newValue;
            return jsonObject.ToString();
        }

        /// <summary>
        /// Gets the value of a specified key in a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="key">The key whose value needs to be retrieved.</param>
        /// <returns>The value of the specified key as a string, or null if the JSON is not an object or the key is not found.</returns>
        public string GetValueInJsonString(string json, string key)
        {
            if (string.IsNullOrEmpty(json))
                return null;

            try
            {
                var jsonToken = JToken.Parse(json);

                if (jsonToken is JObject jsonObject)
                {
                    return jsonObject.GetValue(key, StringComparison.OrdinalIgnoreCase)?.Value<string>();
                }
            }
            catch (JsonReaderException)
            {
                return null;
            }

            return null;
        }


        /// <summary>
        /// Adds a new JSON object value to the specified key in a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="key">The key to which the value should be added.</param>
        /// <param name="value">The new JSON object value to add.</param>
        /// <returns>The modified JSON string.</returns>
        public string AddNewJsonValue(string json, string key, JObject value)
        {
            var jsonObject = JObject.Parse(json);
            jsonObject[key] = value;
            return jsonObject.ToString();
        }

        /// <summary>
        /// Adds a new string value to the specified key in a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="key">The key to which the value should be added.</param>
        /// <param name="value">The new string value to add.</param>
        /// <returns>The modified JSON string.</returns>
        public string AddNewStringValue(string json, string key, string value)
        {
            var jsonObject = JObject.Parse(json);
            jsonObject[key] = value;
            return jsonObject.ToString();
        }

        /// <summary>
        /// Deletes the value of the specified key in a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="key">The key whose value needs to be deleted.</param>
        /// <returns>The modified JSON string.</returns>
        public string DeleteStringValue(string json, string key)
        {
            return _jsonElementRemover.RemoveElementsByValue(json, key);
        }

        /// <summary>
        /// Changes the key of a value in a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="oldKey">The old key whose value needs to be changed.</param>
        /// <param name="newKey">The new key to set for the specified value.</param>
        /// <returns>The modified JSON string.</returns>
        public string ChangeKey(string json, string oldKey, string newKey)
        {
            var jsonObject = JObject.Parse(json);
            var value = GetValueInJsonString(json, oldKey);
            jsonObject[newKey] = value;
            var returnjson = JsonConvert.SerializeObject(jsonObject);
            return _jsonElementRemover.RemoveElementsByValue(returnjson, oldKey);
        }
    }

}
