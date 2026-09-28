using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// <see cref="IImportDocument"/> over a JSON object (<see cref="JObject"/>). Property lookups are case-insensitive so
    /// mapping output names match regardless of casing differences in the file.
    /// </summary>
    public class JsonImportDocument : IImportDocument
    {
        private readonly JObject _object;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonImportDocument"/> class.
        /// </summary>
        /// <param name="node">The JSON object node this document wraps.</param>
        public JsonImportDocument(JObject node)
        {
            _object = node ?? new JObject();
        }

        private JToken Find(string name) =>
            _object.Properties()
                .FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))?.Value;

        /// <inheritdoc />
        public string GetAttributeValue(string name)
        {
            var token = Find(name);
            if (token == null || token.Type == JTokenType.Null || token.Type == JTokenType.Object || token.Type == JTokenType.Array)
            {
                return null;
            }
            return token.ToString();
        }

        /// <inheritdoc />
        public IEnumerable<IImportDocument> GetArrayItems(string arrayName)
        {
            var token = Find(arrayName);
            if (token is JArray array)
            {
                return array.OfType<JObject>().Select(o => (IImportDocument)new JsonImportDocument(o)).ToList();
            }
            if (token is JObject single)
            {
                return new List<IImportDocument> { new JsonImportDocument(single) };
            }
            return Enumerable.Empty<IImportDocument>();
        }

        /// <inheritdoc />
        public IImportDocument GetChildObject(string name)
        {
            return Find(name) is JObject child ? new JsonImportDocument(child) : null;
        }
    }
}
