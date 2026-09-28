using arc.common.Models.Monitoring;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

namespace arc.common.Utils
{
    public class JsonWholeStructureFieldsCollector : IJsonWholeStructureFieldsCollector
    {
        /// <summary>
        /// Language tag a JSON <c>true</c> is collected as, so a stored flag reads as Yes in the caller's language.
        /// </summary>
        private const string YesTag = "@GenYesA@";

        /// <summary>
        /// Language tag a JSON <c>false</c> is collected as.
        /// </summary>
        private const string NoTag = "@GenNo@";

        private JToken _json;
        private Dictionary<string,string> _fieldList;

        public Dictionary<string, string> GetFields()
        {
            return _fieldList;
        }

        public List<JsonItemModel> GetStructure(string message)
        {
            using (var sr = new StringReader(message))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                _json = JToken.ReadFrom(jr);
            }

            _fieldList = new Dictionary<string, string>();

            return GetLevel(_json);
        }

        public List<JsonArrayModel> GetJsonArray(string message)
        {
            using (var sr = new StringReader(message))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                _json = JToken.ReadFrom(jr);
            }

            _fieldList = new Dictionary<string, string>();

            return GetArrayContents(_json);
        }

        private List<JsonItemModel> GetLevel(JToken jToken)
        {
           return GetJsonObject(jToken);
        }

        private List<JsonArrayModel> GetArrayContents(JToken jToken)
        {
            var itemList = new List<JsonArrayModel>();

            foreach (var child in jToken.Children())
            {
                var newItem = new JsonArrayModel { ChildItems = GetJsonObject(child) };
                itemList.Add(newItem);
            }

            return itemList;
        }

        /// <summary>
        /// Reads every property of a JSON object into <see cref="JsonItemModel"/> entries, recursing into
        /// nested objects and arrays. <see cref="JsonItemModel.Key"/> keeps the original property name so
        /// callers that later translate <see cref="JsonItemModel.Label"/> can still identify the field.
        /// </summary>
        /// <param name="jToken">Object token to read.</param>
        /// <returns>One entry per property, in document order.</returns>
        private List<JsonItemModel> GetJsonObject(JToken jToken)
        {
            var returnValue = new List<JsonItemModel>();

            foreach (var child in jToken.Children<JProperty>())
            {
                var key = child.Name;
                var value = child.Value;
                var newField = new JsonItemModel
                {
                    Key = key,
                    Label = key
                };

                if (value.Type == JTokenType.String || value.Type == JTokenType.Integer || value.Type == JTokenType.Float)
                {
                    newField.Contents = value.Value<string>();
                    if (! _fieldList.ContainsKey(newField.Label))
                    {
                        _fieldList.Add(newField.Label, newField.Contents);
                    }
                }

                if (value.Type == JTokenType.Boolean)
                {
                    newField.Contents = value.Value<bool>() ? YesTag : NoTag;
                    if (! _fieldList.ContainsKey(newField.Label))
                    {
                        _fieldList.Add(newField.Label, newField.Contents);
                    }
                }

                if (value.Type == JTokenType.Object)
                {
                    newField.ChildItems = GetJsonObject((JToken)value);
                }

                if (value.Type == JTokenType.Array)
                {
                    newField.ArrayItems = GetArrayContents((JToken)value);
                }
                returnValue.Add(newField);
            }

            return returnValue;
        }
    }
}
