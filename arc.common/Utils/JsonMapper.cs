using arc.common.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace arc.common.Utils
{
    public class JsonMapper : IJsonMapper 
    {
        public string FilterJustRequired (string sourceJson, string[] requiredFields)
        {
            sourceJson = sourceJson.Replace("{", "").Replace("}", "");
            var sourceArray = sourceJson.Split(",");
            var mappedFields = "{";

            foreach (var source in sourceArray)
            {
                var elements = source.Split(":");
                foreach (var target in requiredFields)
                {
                    if (target.Trim().ToLower() == elements[0].Replace("'", "").Trim().ToLower())
                    {
                        mappedFields += mappedFields.Length > 1 ? "," + source : source;
                    }
                }
            }

            mappedFields += "}";

            return mappedFields;
        }

        public string Transform (string sourceJson, string transformRules, bool includeAllFields = false) 
        {
            sourceJson = sourceJson.Replace("{", "").Replace("}", "");
            var sourceArray = sourceJson.Split(",");
            var mappedFields = "{";
            var rules = JsonConvert.DeserializeObject<List<TransformRulesModel>>(transformRules);

            foreach (var source in sourceArray)
            {
                var elements = source.Split(":");
                var elementChanged = false;
                foreach (var rule in rules)
                {
                    if (rule.Type != "New")
                    {
                        var element = elements[0].Replace("'", "").Replace("\r\n", "").Replace("\"", "").Trim().ToLower();
                        if (rule.Source.Trim().ToLower() == element)
                        {
                            if (rule.Type == "Mapping")
                            {
                                if (rule.Source.Trim().ToLower() == element)
                                {
                                    var newField = string.IsNullOrEmpty(rule.Value) ? rule.Target + ":" + elements[1] : rule.Target + ":'" + rule.Value + "'";
                                    mappedFields += mappedFields.Length > 1 ? "," + newField : newField;
                                    elementChanged = true;
                                }
                            }
                            if (rule.Type == "Remove")
                            {
                                elementChanged = true;
                            }
                        }
                    }
                }

                if (includeAllFields && ! elementChanged)
                {
                    mappedFields += mappedFields.Length > 1 ? "," + source : source;
                }
            }

            foreach(var rule in rules)
            {
                if (rule.Type == "New")
                {
                    var newField = rule.Target + ":'" + rule.Value + "'";
                    mappedFields += mappedFields.Length > 1 ? "," + newField : newField;
                }
            }

            mappedFields += "}";

            return mappedFields;
        }
    }
}
