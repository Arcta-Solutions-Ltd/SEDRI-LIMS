using arc.common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Contains set of static methods to help with json string manipulation.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Prepares an event message for queue storage. Valid JSON is returned unchanged;
        /// legacy single-quote payloads are normalized only when parsing fails.
        /// </summary>
        /// <param name="message">Raw event message JSON from the client.</param>
        /// <returns>JSON suitable for queue insert and deserialization.</returns>
        public static string NormalizeForQueueStorage(this string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return message;
            }

            try
            {
                JToken.Parse(message);
                return message;
            }
            catch (JsonReaderException)
            {
                return message.Replace("'", "\"");
            }
        }

        /// <summary>
        /// Extension method to remove null values from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string from which null values should be removed.</param>
        /// <returns>The JSON string with null values removed.</returns>
        public static string RemoveNullsFromJsonString(this string json)
        {
            var jsonElementRemover = new JsonElementRemover();

            var jsonConverter = new JsonWholeStructureFieldsCollector();

            var fields = jsonConverter.GetStructure(json);

            var nullfields = fields.Where(f => f.Contents == "null");

            foreach (var field in nullfields)
            {
                json = jsonElementRemover.RemoveElementsByValue(json, field.Label);
            }

            return json;
        }

        /// <summary>
        /// Removes the specified properties from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string to modify.</param>
        /// <param name="elements">The property names to remove.</param>
        /// <returns>The JSON string with the specified properties removed.</returns>
        public static string RemoveElements(this string json, params string[] elements)
        {
            JObject newObject = JObject.Parse(json);
            foreach (var element in elements)
            {
                if (newObject.Property(element) != null)
                {
                    newObject.Remove(element);
                }
            }
            return newObject.ToString();
        }

        /// <summary>
        /// Extracts an integer value from a JSON string based on the specified attribute name.
        /// </summary>
        /// <param name="json">The JSON string containing the attribute.</param>
        /// <param name="attributeName">The name of the attribute to extract.</param>
        /// <returns>The integer value of the attribute, or null if not found or not a valid integer.</returns>
        public static int? GetIntFromJson(this string json, string attributeName)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty(attributeName, out JsonElement element) && element.TryGetInt32(out int value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// Retrieves a string value from a JSON string based on the given attribute name.
        /// Handles Number and other types by converting to string (e.g. Id, TagId from combobox).
        /// </summary>
        /// <param name="json">The JSON string.</param>
        /// <param name="attributeName">The attribute name to search for.</param>
        /// <returns>The string value if found; otherwise, null.</returns>
        public static string GetStringFromJson(this string json, string attributeName)
        {
            using JsonDocument doc = JsonDocument.Parse(json);

            if (doc.RootElement.TryGetProperty(attributeName, out JsonElement element))
            {
                return element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.TryGetInt64(out var n) ? n.ToString() : element.GetDouble().ToString(System.Globalization.CultureInfo.InvariantCulture),
                    JsonValueKind.True => "true",
                    JsonValueKind.False => "false",
                    JsonValueKind.Null => null,
                    _ => null
                };
            }

            return null;
        }

        /// <summary>
        /// Safely converts a JToken to a nullable DateTime, handling null/undefined and the string "null".
        /// </summary>
        /// <param name="token">The token to convert.</param>
        /// <returns>A nullable DateTime or null if not parsable/present.</returns>
        public static System.DateTime? AsNullableDateTime(this JToken token)
        {
            return token?.Type switch
            {
                JTokenType.Null => null,
                JTokenType.Undefined => null,
                JTokenType.String =>
                    !string.IsNullOrWhiteSpace(token.Value<string>()) &&
                    !string.Equals(token.Value<string>(), "null", System.StringComparison.OrdinalIgnoreCase) &&
                    System.DateTime.TryParse(token.Value<string>(), out var dt)
                        ? dt
                        : (System.DateTime?)null,
                _ => token?.ToObject<System.DateTime?>()
            };
        }

        /// <summary>
        /// Retrieves a nullable DateTime from a JObject property with robust null handling.
        /// </summary>
        /// <param name="obj">The JObject to read from.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A nullable DateTime or null.</returns>
        public static System.DateTime? GetNullableDateTime(this JObject obj, string propertyName)
        {
            if (obj == null) return null;
            var token = obj[propertyName];
            return token.AsNullableDateTime();
        }

        /// <summary>
        /// Retrieves a nullable integer from a JObject property. Returns null when the property
        /// is missing, null, empty string, or not a valid integer (DB allows null for optional IDs).
        /// Handles both JTokenType.Integer and JTokenType.String (e.g. from form/mapper output).
        /// </summary>
        /// <param name="obj">The JObject to read from.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>int? when present and valid; otherwise null.</returns>
        public static int? GetNullableInt32(this JObject obj, string propertyName)
        {
            if (obj == null) return null;
            var token = obj[propertyName] ?? obj[propertyName.ToLowerInvariant()];
            if (token == null || token.Type == JTokenType.Null || token.Type == JTokenType.Undefined) return null;
            if (token.Type == JTokenType.Integer) return token.Value<int?>();
            if (token.Type == JTokenType.String)
            {
                var s = token.Value<string>();
                if (string.IsNullOrWhiteSpace(s) || s.Equals("null", StringComparison.OrdinalIgnoreCase)) return null;
                if (int.TryParse(s, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var v)) return v;
            }
            return null;
        }

        /// <summary>
        /// Retrieves a string value from a JObject property. Returns null when missing or null.
        /// </summary>
        /// <param name="obj">The JObject to read from.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>string or null.</returns>
        public static string GetString(this JObject obj, string propertyName)
        {
            if (obj == null) return null;
            var token = obj[propertyName];
            return token?.Type == JTokenType.Null || token?.Type == JTokenType.Undefined
                ? null
                : token?.Value<string>();
        }

        /// <summary>
        /// Rebuilds a JSON object so its top-level properties appear first in <paramref name="orderedKeys"/> order
        /// (case-insensitive key match), then any remaining properties in their original order.
        /// Does not recurse into nested objects or arrays.
        /// </summary>
        /// <param name="json">JSON string whose root must be an object for reordering; arrays, primitives, null, or invalid JSON are returned unchanged.</param>
        /// <param name="orderedKeys">Desired property names in order. Keys missing from the source object are skipped.</param>
        /// <returns>Compact JSON string for a root object, or the original <paramref name="json"/> when reordering does not apply.</returns>
        public static string ReorderPropertiesByKeyOrder(this string json, IReadOnlyList<string> orderedKeys)
        {
            if (string.IsNullOrWhiteSpace(json) || orderedKeys == null || orderedKeys.Count == 0)
                return json;

            try
            {
                var token = JToken.Parse(json);
                if (token.Type != JTokenType.Object)
                    return json;

                return (token as JObject).ReorderPropertiesByKeyOrder(orderedKeys).ToString(Formatting.None);
            }
            catch (JsonReaderException)
            {
                return json;
            }
        }

        /// <summary>
        /// Rebuilds a <see cref="JObject"/> so its properties appear first in <paramref name="orderedKeys"/> order
        /// (case-insensitive key match), then any remaining properties in their original order.
        /// Values are deep-cloned. Does not recurse into nested objects or arrays beyond cloning token values.
        /// </summary>
        /// <param name="source">Source object to reorder; if null, returns null.</param>
        /// <param name="orderedKeys">Desired property names in order. Keys missing from <paramref name="source"/> are skipped.</param>
        /// <returns>A new <see cref="JObject"/> with properties reordered, or <paramref name="source"/> when <paramref name="orderedKeys"/> is null or empty.</returns>
        public static JObject ReorderPropertiesByKeyOrder(this JObject source, IReadOnlyList<string> orderedKeys)
        {
            if (source == null || orderedKeys == null || orderedKeys.Count == 0)
                return source;

            var result = new JObject();
            var added = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var key in orderedKeys)
            {
                if (string.IsNullOrEmpty(key))
                    continue;

                var prop = source.Properties()
                    .FirstOrDefault(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase));
                if (prop == null || !added.Add(prop.Name))
                    continue;

                result.Add(prop.Name, prop.Value.DeepClone());
            }

            foreach (var prop in source.Properties())
            {
                if (added.Contains(prop.Name))
                    continue;
                result.Add(prop.Name, prop.Value.DeepClone());
                added.Add(prop.Name);
            }

            return result;
        }

        /// <summary>
        /// Walks a dot separated property path with optional array indexers and returns the token found there.
        /// Property names are matched case-insensitively so stored payloads that differ in casing from the
        /// configured path still resolve. Supports paths such as <c>Crafted[0].Contents[0].value</c>,
        /// including bare indexers on the path root (for example <c>[0].value</c>).
        /// </summary>
        /// <param name="token">Token to start from; when null, null is returned.</param>
        /// <param name="path">Path to walk. When null or whitespace, <paramref name="token"/> is returned unchanged.</param>
        /// <returns>The token at <paramref name="path"/>, or null when any segment or index cannot be resolved.</returns>
        public static JToken SelectTokenByPath(this JToken token, string path)
        {
            if (token == null)
                return null;

            if (string.IsNullOrWhiteSpace(path))
                return token;

            var current = token;

            foreach (var segment in path.Split('.', StringSplitOptions.RemoveEmptyEntries))
            {
                var indexerStart = segment.IndexOf('[');
                var propertyName = indexerStart < 0 ? segment : segment[..indexerStart];

                if (!string.IsNullOrEmpty(propertyName))
                {
                    if (current is not JObject currentObject)
                        return null;

                    var property = currentObject.Properties()
                        .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
                    if (property == null)
                        return null;

                    current = property.Value;
                }

                if (indexerStart < 0)
                    continue;

                current = WalkIndexers(current, segment[indexerStart..]);
                if (current == null)
                    return null;
            }

            return current;
        }

        /// <summary>
        /// Applies a run of array indexers such as <c>[0][2]</c> to a token.
        /// </summary>
        /// <param name="token">Token the indexers apply to.</param>
        /// <param name="indexers">Indexer text starting at the first <c>[</c>.</param>
        /// <returns>The indexed token, or null when the text is malformed or an index is out of range.</returns>
        private static JToken WalkIndexers(JToken token, string indexers)
        {
            var current = token;
            var position = 0;

            while (position < indexers.Length)
            {
                if (indexers[position] != '[')
                    return null;

                var close = indexers.IndexOf(']', position);
                if (close < 0)
                    return null;

                var indexText = indexers[(position + 1)..close];
                if (!int.TryParse(indexText, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var index))
                    return null;

                if (current is not JArray currentArray || index < 0 || index >= currentArray.Count)
                    return null;

                current = currentArray[index];
                position = close + 1;
            }

            return current;
        }
    }
}
