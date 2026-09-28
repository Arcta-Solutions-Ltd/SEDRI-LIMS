using arc.common.Models;
using System.Collections.Generic;

namespace arc.domain.Configuration.MappingsConfig
{
    public class RepeaterConfig
    {
        private readonly TokenInfoModel _tokenInfoModel;

        public string Target { get; set; }
        public string Name { get; set; }
        public List<MapperRulesConfig> Rules { get; set; } = new List<MapperRulesConfig>();

        public RepeaterConfig(TokenInfoModel tokenInfoModel)
        {
            _tokenInfoModel = tokenInfoModel;
        }

        public string CreateRepeaterTarget(List<JsonFieldModel> fields)
        {
            var replacer = new ReplacementString(_tokenInfoModel);
            var index = 0;
            var found = true;
            var returnString = "[";

            while (found)
            {
                found = false;
                var currentTarget = Target;
                foreach (var rule in Rules)
                {
                    var replacementString = replacer.GetValue(fields, rule, index);

                    if (replacementString != "")
                    {
                        found = true;
                    }
                    currentTarget = currentTarget.Replace(rule.Key, replacementString);
                }

                if (found)
                {
                    returnString += returnString == "[" ? currentTarget : "," + currentTarget;
                }
                index++;
            }

            returnString += "]";
           
            return returnString;
        }

        public void AddRule(string key, string type, string source, string value)
        {
            var newRule = new MapperRulesConfig { Key = key, Type = type, Source = source, Value = value };
            Rules.Add(newRule);
        }
    }
}
