using arc.common.Models;
using arc.common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.MappingsConfig
{
    public class ReplacementString
    {
        private readonly TokenInfoModel _tokenInfoModel;

        public ReplacementString(TokenInfoModel tokenInfoModel)
        {
            _tokenInfoModel = tokenInfoModel;
        }

        public string GetValue(List<JsonFieldModel> fields, MapperRulesConfig rule, int index, List<JsonFieldModel> localFields = null)
        {
            var replacementString = "";

            var field = new List<JsonFieldModel>();

            if (index == -1)
            {
                if (string.IsNullOrEmpty(rule.Where))
                {
                    field = fields.Where(f => f.Key.ToLower() == rule.Source.ToLower()).ToList();
                }
                else
                {
                    field = fields.Where(f => f.Key.ToLower() == rule.Source.ToLower() && f.Value.ToLower() == rule.Where.ToLower()).ToList();
                }
            } else
            {
                if (string.IsNullOrEmpty(rule.Where))
                {
                    field = fields.Where(f => f.Index == index && f.Key.ToLower() == rule.Source.ToLower()).ToList();
                }
                else
                {
                    field = fields.Where(f => f.Index == index && f.Key.ToLower() == rule.Source.ToLower() && f.Value.ToLower() == rule.Where.ToLower()).ToList();
                }
            }

            if (rule.Value != null && rule.Value.Length >= 8 && rule.Value[..8] == "<:Local.")
            {
                var end = rule.Value.Length - 2;
                var placeholder = rule.Value[8..end];

                if (localFields.Count() > 0)
                {
                    var valueField = localFields.Where(f => f.Key.ToLower() == placeholder.ToLower()).ToList();
                    if (valueField.Count() > 0)
                    {
                        replacementString = valueField[0].Value;
                    }
                }
            } else
            {
                switch (rule.Value)
                {
                    case "<:now:>":
                        replacementString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        break;
                    case "<:username:>":
                        replacementString = _tokenInfoModel.Username;
                        break;
                    default:

                        if (field.Count() > 0)
                        {
                            var valueField = fields.Where(f => f.Key.ToLower() == rule.Value.ToLower() && f.Index == field[0].Index).ToList();
                            if (valueField.Count() > 0)
                            {
                                replacementString = valueField[0].Value;
                            }
                        }
                        break;
                }
            }

            return replacementString;
        }
    }
}
