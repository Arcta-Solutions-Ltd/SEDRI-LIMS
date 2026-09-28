using arc.common;
using arc.common.Models;
using arc.common.Models.Tests;
using arc.common.Models.User;
using arc.common.Utils;
using arc.domain.Configuration.PagesConfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace arc.domain.Configuration.MappingsConfig
{
    public class MapperConfig : IMap
    {
        private TokenInfoModel _tokenInfoModel;

        public MapperConfig(TokenInfoModel tokenInfoModel)
        {
            _tokenInfoModel = tokenInfoModel;
        }

        private string _target;
        
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IncludeIfNotFound { get; set; }
        public bool PassthroughIfNoRule { get; set; }
        public List<MapperRulesConfig> Rules { get; set; } = new List<MapperRulesConfig>();
        public List<RepeaterConfig> Repeaters { get; set; } = new List<RepeaterConfig>();
        public string JsonFields { get; set; }

        public List<JsonFieldModel> _localFields { get; set; }

        public List<JsonFieldModel> GetRequiredLocalFields()
        {
            var fieldList = new List<JsonFieldModel>();

            foreach (var rule in Rules)
            {
                if (rule.Value != null && rule.Value.ToLower().StartsWith("<:local."))
                {
                    var end = rule.Value.Length - 2;

                    var item = new JsonFieldModel
                    {
                        Key = rule.Value[8..end]
                    };

                    fieldList.Add(item);
                }
            }
            return fieldList;
        }

        public void SetRequiredLocalFields(List<JsonFieldModel> localFields)
        {
            _localFields = localFields;
        }

        public JObject GetTarget()
        {
            return JObject.Parse(_target);
        }

        public void SetTokenInfo(TokenInfoModel tokenInfoModel)
        {
            _tokenInfoModel = tokenInfoModel;
        }

        public void SetTarget(string newTarget)
        {
            _target = newTarget;
        }

        public string Map(string source)
        {
            JToken json;
            using (var sr = new StringReader(source))
            using (var jr = new JsonTextReader(sr) { DateParseHandling = DateParseHandling.None })
            {
                json = JToken.ReadFrom(jr);
            }

            if (json.Type == JTokenType.Array)
            {
                string compoundTarget = "[";
                var index = 0;
                // Call mapper for every object in the list.
                foreach (var child in json.Children())
                {
                    string individualTarget = MapSingleItem(child, _target);
                    compoundTarget += (index > 0 ? "," : "") + individualTarget;
                    index++;
                }
                _target = compoundTarget + "]";
            }
            else
            {
                // Map the only available object.
                _target = MapSingleItem(json, _target);
            }

            return _target;
        }

        public string MapSingleItem(JToken json, string target)
        {
            var fieldsCollector = new JsonFieldsCollector(JsonFields);
            var fields = fieldsCollector.GetAllFields(json);

            var repeaterList = new Dictionary<string, string>();
            if (Repeaters != null)
            {
                foreach (var repeater in Repeaters)
                {
                    var repeaterContent = repeater.CreateRepeaterTarget(fields);
                    repeaterList.Add(repeater.Name, repeaterContent);
                }
            }

            var replacer = new ReplacementString(_tokenInfoModel);

            foreach (var rule in Rules)
            {
                var replacementString = "";
                if (rule.Type.ToLower() == "repeater")
                {
                    var rep = repeaterList.Where(r => r.Key == rule.Key).ToList();
                    replacementString = rep.First().Value;
                    replacementString = replacementString.Replace("\"", "'");
                }
                else
                {
                    replacementString = replacer.GetValue(fields, rule, -1, _localFields);
                }

                if (replacementString != "")
                {
                    if (rule.Type.ToLower() == "repeater")
                    {
                        target = target.Replace('\'' + rule.Key + '\'', replacementString).Replace('\"' + rule.Key + '\"', replacementString).Replace("\\\"" + rule.Key + "\\\"", replacementString);
                    }
                    else
                    {
                        var nullReplace = IncludeIfNotFound ? "null" : "<:remove:>";
                        replacementString = replacementString == null ? nullReplace : EscapeForJsonString(replacementString);
                        target = target.Replace(rule.Key, replacementString);
                    }
                }
                else if (IncludeIfNotFound)
                {
                    target = target.Replace(rule.Key, "null");
                }
                else
                {
                    target = target.Replace(rule.Key, "<:remove:>");
                }
            }

            if (PassthroughIfNoRule)
            {
                foreach (var field in fields)
                {
                    var fieldRule = Rules.Where(r => r.Value.ToLower() == field.Key.ToLower());
                    if (fieldRule.Count() == 0)
                    {
                        target = target.Remove(target.Length - 1);
                        target += (target.Length > 1 ? "," : "") + '"' + field.Key + "\":\"" + field.Value + "\"}";
                    }
                }
            }

            var jsonRemover = new JsonElementRemover();
            target = jsonRemover.RemoveElementsByValue(target, "<:remove:>");

            return target;
        }

        public void LoadMapperDefinition(string json)
        {
            var newjson = JToken.Parse(json);
            var fieldsCollector = new JsonFieldsCollector();
            _target = fieldsCollector.GetNamedObject(newjson, "target");
        }

        public void ChangeFieldName(string oldName, string newName, string type)
        {
            var jsonElementRemover = new JsonElementRemover();
            var jsonReplacer = new JsonReplacer(jsonElementRemover);

            foreach (var rule in Rules)
            {
                if (!string.IsNullOrWhiteSpace(rule.Source))
                {
                    rule.ChangeFieldName(oldName, newName);
                }
            }

            var target = JsonConvert.DeserializeObject<TestResultModel>(_target);

            if (target.TestResults != null)
            {
                target.TestResults = jsonReplacer.ChangeKey(target.TestResults, oldName, newName);
                _target = JsonConvert.SerializeObject(target);
            }
            else
            {
                _target = jsonReplacer.ChangeKey(_target, oldName, newName);
            }

        }

        public void AddField(string id, string type, List<FieldGridConfig> gridFields)
        {
            var lastUsedRuleNumber = GetNextRuleNumber();
            var newRuleEntry = new MapperRulesConfig();

            if (type == "fieldgrid")
            {
                if (gridFields.Count > 0)
                {
                    newRuleEntry = new MapperRulesConfig
                    {
                        Key = "<:" + lastUsedRuleNumber + ":>",
                        Type = "Repeater"
                    };

                    var target = "{}";
                    var repeater = new RepeaterConfig(_tokenInfoModel) { Name = newRuleEntry.Key };
                    var gridLine = 1;
                    foreach (var gridItem in gridFields)
                    {
                        var key = "<:" + lastUsedRuleNumber + gridLine + ":>";
                        repeater.AddRule(key, gridItem.Type, gridItem.Id, gridItem.Id);
                        var separator = target == "{}" ? "" : ",";
                        target = target.Replace("}", separator + "'" + gridItem.Id + "':'" + key + "'}");
                        gridLine++;
                    }

                    repeater.Target = target;
                    Repeaters.Add(repeater);

                }
            }
            else
            {
                newRuleEntry = new MapperRulesConfig
                {
                    Key = "<:" + lastUsedRuleNumber + ":>",
                    Source = id,
                    Value = id,
                    Type = "Mapping"
                };
            }

            var jsonElementRemover = new JsonElementRemover();
            var jsonReplacer = new JsonReplacer(jsonElementRemover);

            if (type == "fieldgrid")
            {
                var target = JsonConvert.DeserializeObject<TestResultModel>(_target);

                if (target.TestResults != null)
                {

                    target.TestResults = jsonReplacer.AddNewStringValue(target.TestResults, id, newRuleEntry.Key);
                    _target = JsonConvert.SerializeObject(target);
                }
                else
                {
                    _target = jsonReplacer.AddNewStringValue(_target, id, newRuleEntry.Key);
                }
            }
            else
            {
                var target = JsonConvert.DeserializeObject<TestResultModel>(_target);
                if (!string.IsNullOrWhiteSpace(target.TestResults))
                {
                    target.TestResults = jsonReplacer.AddNewStringValue(target.TestResults, id, newRuleEntry.Key);
                    _target = JsonConvert.SerializeObject(target);
                }
                else
                {
                    _target = jsonReplacer.AddNewStringValue(_target, id, newRuleEntry.Key);

                }
            }

            Rules.Add(newRuleEntry);
        }

        public void DeleteField(string source, string type)
        {
            var ruleToDelete = new MapperRulesConfig();
            var jsonElementRemover = new JsonElementRemover();

            var target = JsonConvert.DeserializeObject<TestResultModel>(_target);
            var jsonReplacer = new JsonReplacer(jsonElementRemover);

            if (type == "fieldgrid")
            {
                if (target.TestResults != null)
                {
                    var mapperTag = jsonReplacer.GetValueInJsonString(target.TestResults, source);
                    //var mapperTag = jsonUtils.GetSingleFieldValue(target.TestResults, source);
                    DeleteRepeater(mapperTag);
                    target.TestResults = jsonElementRemover.RemoveElementsByValue(target.TestResults, source);
                    _target = JsonConvert.SerializeObject(target);
                    ruleToDelete = Rules.FirstOrDefault(d => d.Key.ToLower() == mapperTag.ToLower());
                }
                else
                {
                    //var mapperTag = jsonUtils.GetSingleFieldValue(_target, source);
                    var mapperTag = jsonReplacer.GetValueInJsonString(_target, source);
                    DeleteRepeater(mapperTag);
                    _target = jsonElementRemover.RemoveElementsByValue(_target, source);
                    ruleToDelete = Rules.FirstOrDefault(d => d.Key.ToLower() == mapperTag.ToLower());
                }
            }
            else
            {
                if (target.TestResults != null)
                {

                    target.TestResults = jsonReplacer.DeleteStringValue(target.TestResults, source);
                    _target = JsonConvert.SerializeObject(target);
                }
                else
                {
                    _target = jsonElementRemover.RemoveElementsByValue(_target, source);
                }
                ruleToDelete = Rules.FirstOrDefault(d => d.Source != null && d.Source.ToLower() == source.ToLower());
            }

            if (ruleToDelete != null)
            {
                Rules.Remove(ruleToDelete);
            }
        }

        private void DeleteRepeater(string name)
        {
            var repeater = Repeaters.FirstOrDefault(r => r.Name.ToLower() == name.ToLower());
            if (repeater != null)
            {
                Repeaters.Remove(repeater);
            }
        }

        /// <summary>
        /// Escapes a string so it can be safely inserted into a JSON string value.
        /// Handles quotes, backslashes, and other characters that would break JSON parsing.
        /// </summary>
        /// <param name="value">The raw string value to escape.</param>
        /// <returns>The escaped string suitable for insertion inside a JSON string.</returns>
        private static string EscapeForJsonString(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var serialized = JsonConvert.SerializeObject(value);
            return serialized.Substring(1, serialized.Length - 2);
        }

        private int GetNextRuleNumber()
        {
            var lastUsedRuleNumber = 0;
            if (Rules != null && Rules.Count() > 0)
            {
                lastUsedRuleNumber = int.Parse(Rules.Last().Key.Replace(":", "").Replace("<", "").Replace(">", ""));
                lastUsedRuleNumber = lastUsedRuleNumber + 1;
            }
            return lastUsedRuleNumber;
        }

        public int AddRule(string id)
        {
            var lastUsedRuleNumber = GetNextRuleNumber();
            var newRuleEntry = new MapperRulesConfig
            {
                Key = "<:" + lastUsedRuleNumber + ":>",
                Source = id,
                Value = id,
                Type = "Mapping"
            };
            Rules.Add(newRuleEntry);
            return lastUsedRuleNumber;
        }

    }
}

