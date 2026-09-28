using arc.common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils
{
    public class JsonFieldsCollector : IJsonFieldsCollector
    {
        private List<JsonFieldModel> fields;
        private string _rememberedValue;
        private string _jsonFields;
        private string _stringFields;

        public JsonFieldsCollector(string jsonFields = "", string stringFields = "")
        {
            fields = new List<JsonFieldModel>();
            _jsonFields = jsonFields;
            _stringFields = stringFields;
        }

        private void CollectFields(JToken jToken)
        {
            switch (jToken.Type)
            {
                case JTokenType.Object:
                    foreach (var child in jToken.Children<JProperty>())
                        CollectFields(child);
                    break;
                case JTokenType.Array:
                    foreach (var child in jToken.Children())
                        CollectFields(child);
                    break;
                case JTokenType.Property:
                    var path = jToken.Path;
                    if (!string.IsNullOrEmpty(_stringFields) && _stringFields.ToLower().Contains(path.ToLower()))
                    {
                        var temp = jToken.First.ToString();
                        var newStringField = new JsonFieldModel
                        {
                            Value = temp,
                            Key = path
                        };
                        fields.Add(newStringField);
                    } else
                    {
                        CollectFields(((JProperty)jToken).Value);
                    }
                    break;
                default:
                    var key = jToken.Path;
                    int index = -1;
                    if (jToken.Path.Contains("["))
                    {
                        // Parent is JProperty when value is inside object; JArray when value is primitive in array.
                        var parent = jToken.Parent;
                        if (parent is JProperty jp)
                            key = jp.Name;
                        else if (parent is JArray arr && arr.Parent is JProperty arrProp)
                            key = arrProp.Name;
                        var indexString = jToken.Path.Substring(jToken.Path.LastIndexOf("[") + 1, jToken.Path.LastIndexOf("]") - jToken.Path.LastIndexOf("[") - 1);
                        index = int.Parse(indexString);
                    }

                    if (! string.IsNullOrEmpty(_jsonFields) && _jsonFields.ToLower().Contains(key.ToLower()))
                    {
                        var jsonString = (string)jToken;
                        if (! string.IsNullOrEmpty(jsonString) && jsonString != "{}")
                        {
                            var json = JToken.Parse((string)jToken);
                            foreach (var child in json.Children<JProperty>())
                                CollectFields(child);
                        }
                    }

                    var newField = new JsonFieldModel
                    {
                        Value = (string)jToken,
                        Key = key,
                        Index = index
                    };
                    fields.Add(newField);
                    break;
            }
        }

        public string GetNamedObject(JToken jToken, string name)
        {
            if (jToken.Path.ToLower() == name.ToLower())
            {
                _rememberedValue = JsonConvert.SerializeObject(jToken.First);
            }
            else
            {
                switch (jToken.Type)
                {
                    case JTokenType.Object:
                        foreach (var child in jToken.Children<JProperty>())
                            GetNamedObject(child, name);
                        break;
                    case JTokenType.Array:
                        foreach (var child in jToken.Children())
                            GetNamedObject(child, name);
                        break;
                    case JTokenType.Property:
                        GetNamedObject(((JProperty)jToken).Value, name);
                        break;
                    default:
                        break;
                }
            }

            return _rememberedValue;
        }

        public List<JsonFieldModel> GetAllFields(JToken token)
        {
            CollectFields(token);
            return fields.ToList();
        }

    }
}
