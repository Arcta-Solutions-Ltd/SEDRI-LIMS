using arc.app.Common;
using arc.common.Models.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Configuration
{
    /// <summary>
    /// Reads editpagerules event payloads, extracting only the persisted properties and ignoring the
    /// query metadata (state options, field options and read only flags) supplied to the editor.
    /// </summary>
    internal static class PageRulesEventPayloadReader
    {
        /// <summary>
        /// Extracts the Id and PageRules detail from an editpagerules POST body.
        /// </summary>
        /// <param name="json">Raw event JSON from the client.</param>
        /// <param name="logWriter">Optional logger for parse diagnostics.</param>
        /// <returns>A page rules model with only the persisted values populated.</returns>
        public static PageRulesModel Read(string json, ILogWriter logWriter = null)
        {
            var model = new PageRulesModel { PageRules = new PageRuleDetailModel() };

            if (string.IsNullOrWhiteSpace(json))
            {
                logWriter?.LogInfo("PageRulesEventPayloadReader: empty payload", nameof(PageRulesEventPayloadReader), nameof(Read));
                return model;
            }

            JObject root;
            try
            {
                root = JObject.Parse(json);
            }
            catch (JsonReaderException exception)
            {
                logWriter?.LogError(
                    $"PageRulesEventPayloadReader: JSON parse failed. Length={json.Length}, error={exception.Message}",
                    nameof(PageRulesEventPayloadReader),
                    nameof(Read));
                throw;
            }

            model.Id = GetStringProperty(root, "Id");

            var pageRules = GetProperty(root, "PageRules");
            if (pageRules != null && pageRules.Type == JTokenType.Object)
            {
                model.PageRules = pageRules.ToObject<PageRuleDetailModel>() ?? new PageRuleDetailModel();
            }

            model.PageRules.VisibleWhenState = (model.PageRules.VisibleWhenState ?? "").Trim();
            model.PageRules.SetsState = (model.PageRules.SetsState ?? "").Trim();
            model.PageRules.ConditionField = (model.PageRules.ConditionField ?? "").Trim();
            model.PageRules.ConditionRule = string.IsNullOrWhiteSpace(model.PageRules.ConditionRule)
                ? "="
                : model.PageRules.ConditionRule.Trim();
            model.PageRules.ConditionValue = (model.PageRules.ConditionValue ?? "").Trim();
            model.PageRules.StateRules = NormalizeStateRules(model.PageRules);

            var effectCount = model.PageRules.StateRules
                .Select(r => r.Effect ?? "")
                .Where(e => !string.IsNullOrEmpty(e))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Count();

            logWriter?.LogInfo(
                $"PageRulesEventPayloadReader: id='{model.Id}', visibleWhenState='{model.PageRules.VisibleWhenState}', " +
                $"ruleCount={model.PageRules.StateRules.Count}, effectCount={effectCount}",
                nameof(PageRulesEventPayloadReader),
                nameof(Read));

            return model;
        }

        /// <summary>
        /// Returns normalized state rules from the payload, falling back to legacy flat fields when
        /// StateRules is absent or empty.
        /// </summary>
        /// <param name="detail">The page rules detail from the payload.</param>
        /// <returns>Trimmed state rules ready for validation and persistence.</returns>
        internal static List<PageRuleStateRuleModel> GetEffectiveStateRules(PageRuleDetailModel detail)
        {
            return NormalizeStateRules(detail ?? new PageRuleDetailModel());
        }

        /// <summary>
        /// Normalizes state rules from StateRules or legacy flat fields.
        /// </summary>
        /// <param name="detail">The page rules detail to normalize.</param>
        /// <returns>Trimmed state rules.</returns>
        private static List<PageRuleStateRuleModel> NormalizeStateRules(PageRuleDetailModel detail)
        {
            var rules = (detail.StateRules ?? new List<PageRuleStateRuleModel>())
                .Where(r => r != null)
                .Select(NormalizeRule)
                .Where(r => !string.IsNullOrEmpty(r.Effect)
                            || !string.IsNullOrEmpty(r.Field)
                            || !string.IsNullOrEmpty(r.Rule)
                            || !string.IsNullOrEmpty(r.Value))
                .ToList();

            if (rules.Count > 0)
            {
                return rules;
            }

            if (string.IsNullOrEmpty(detail.SetsState))
            {
                return new List<PageRuleStateRuleModel>();
            }

            return new List<PageRuleStateRuleModel>
            {
                NormalizeRule(new PageRuleStateRuleModel
                {
                    Effect = detail.SetsState,
                    Field = detail.ConditionField,
                    Rule = detail.ConditionRule,
                    Value = detail.ConditionValue
                })
            };
        }

        /// <summary>
        /// Trims rule fields and lowercases the effect token.
        /// </summary>
        /// <param name="rule">The rule to normalize.</param>
        /// <returns>A normalized copy of the rule.</returns>
        private static PageRuleStateRuleModel NormalizeRule(PageRuleStateRuleModel rule)
        {
            return new PageRuleStateRuleModel
            {
                Effect = (rule.Effect ?? "").Trim().ToLowerInvariant(),
                Field = (rule.Field ?? "").Trim(),
                Rule = string.IsNullOrWhiteSpace(rule.Rule) ? "=" : rule.Rule.Trim(),
                Value = (rule.Value ?? "").Trim()
            };
        }

        /// <summary>
        /// Returns a property from the payload using a case insensitive name match.
        /// </summary>
        /// <param name="root">The parsed payload.</param>
        /// <param name="propertyName">The property to find.</param>
        /// <returns>The property value, or null when it is absent.</returns>
        private static JToken GetProperty(JObject root, string propertyName)
        {
            return root.Properties()
                .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))
                ?.Value;
        }

        /// <summary>
        /// Returns a string property from the payload using a case insensitive name match.
        /// </summary>
        /// <param name="root">The parsed payload.</param>
        /// <param name="propertyName">The property to find.</param>
        /// <returns>The property value as a string, or null when it is absent.</returns>
        private static string GetStringProperty(JObject root, string propertyName)
        {
            var value = GetProperty(root, propertyName);
            return value == null || value.Type == JTokenType.Null ? null : value.ToString();
        }
    }
}
