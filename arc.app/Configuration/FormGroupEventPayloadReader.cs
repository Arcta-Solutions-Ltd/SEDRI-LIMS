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
    /// Reads editformgroup event payloads, extracting only persisted properties and ignoring query metadata.
    /// </summary>
    internal static class FormGroupEventPayloadReader
    {
        /// <summary>
        /// Extracts Id, Rules, and FieldList from an editformgroup POST body, ignoring extra query metadata.
        /// </summary>
        /// <param name="json">Raw event JSON from the client.</param>
        /// <param name="logWriter">Optional logger for parse diagnostics.</param>
        /// <returns>Form group model with only persisted fields populated.</returns>
        public static FormGroupModel Read(string json, ILogWriter logWriter = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                logWriter?.LogInfo("FormGroupEventPayloadReader: empty payload", "FormGroupEventPayloadReader", "Read");
                return new FormGroupModel();
            }

            JObject root;
            try
            {
                root = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                var hasFieldOptions = json.IndexOf("fieldOptions", StringComparison.OrdinalIgnoreCase) >= 0;
                var fieldListCount = CountArrayItems(json, "FieldList");
                logWriter?.LogError(
                    $"FormGroupEventPayloadReader: JSON parse failed. Length={json.Length}, hasFieldOptions={hasFieldOptions}, fieldListCount={fieldListCount}, error={ex.Message}",
                    "FormGroupEventPayloadReader",
                    "Read");
                throw;
            }

            if (root["fieldOptions"] != null || root["effectOptions"] != null)
            {
                logWriter?.LogInfo(
                    $"FormGroupEventPayloadReader: ignoring query metadata (fieldOptions={root["fieldOptions"] != null}, effectOptions={root["effectOptions"] != null})",
                    "FormGroupEventPayloadReader",
                    "Read");
            }

            var model = new FormGroupModel
            {
                Id = GetStringProperty(root, "Id"),
                Key = GetStringProperty(root, "Key"),
                Rules = root["Rules"]?.ToObject<List<FormGroupRuleModel>>() ?? new List<FormGroupRuleModel>(),
                FieldList = root["FieldList"]?.ToObject<List<FieldListModel>>() ?? new List<FieldListModel>(),
            };

            logWriter?.LogInfo(
                $"FormGroupEventPayloadReader: parsed FieldList count={model.FieldList.Count}, Rules count={model.Rules?.Count ?? 0}",
                "FormGroupEventPayloadReader",
                "Read");

            return model;
        }

        /// <summary>
        /// Extracts Id and Rules from an addformgroup POST body. FieldList and query metadata are ignored
        /// because new subsections are always created empty.
        /// </summary>
        /// <param name="json">Raw event JSON from the client.</param>
        /// <param name="logWriter">Optional logger for parse diagnostics.</param>
        /// <returns>Form group model with Id and Rules only; FieldList is always empty.</returns>
        public static FormGroupModel ReadForAdd(string json, ILogWriter logWriter = null)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                logWriter?.LogInfo("FormGroupEventPayloadReader: empty addformgroup payload", "FormGroupEventPayloadReader", "ReadForAdd");
                return new FormGroupModel();
            }

            JObject root;
            try
            {
                root = JObject.Parse(json);
            }
            catch (JsonReaderException ex)
            {
                var hasFieldOptions = json.IndexOf("fieldOptions", StringComparison.OrdinalIgnoreCase) >= 0;
                var fieldListCount = CountArrayItems(json, "FieldList");
                logWriter?.LogError(
                    $"FormGroupEventPayloadReader: addformgroup JSON parse failed. Length={json.Length}, hasFieldOptions={hasFieldOptions}, fieldListCount={fieldListCount}, error={ex.Message}",
                    "FormGroupEventPayloadReader",
                    "ReadForAdd");
                throw;
            }

            var fieldListToken = root["FieldList"] ?? root.Properties()
                .FirstOrDefault(p => string.Equals(p.Name, "FieldList", StringComparison.OrdinalIgnoreCase))?.Value;
            var fieldListCountInPayload = fieldListToken is JArray jArray ? jArray.Count : 0;
            if (fieldListCountInPayload > 0)
            {
                logWriter?.LogInfo(
                    $"FormGroupEventPayloadReader: addformgroup ignoring FieldList count={fieldListCountInPayload} (new subsections are created empty)",
                    "FormGroupEventPayloadReader",
                    "ReadForAdd");
            }

            if (root["fieldOptions"] != null || root["effectOptions"] != null)
            {
                logWriter?.LogInfo(
                    $"FormGroupEventPayloadReader: addformgroup ignoring query metadata (fieldOptions={root["fieldOptions"] != null}, effectOptions={root["effectOptions"] != null})",
                    "FormGroupEventPayloadReader",
                    "ReadForAdd");
            }

            var model = new FormGroupModel
            {
                Id = GetStringProperty(root, "Id"),
                Key = GetStringProperty(root, "Key"),
                Rules = root["Rules"]?.ToObject<List<FormGroupRuleModel>>() ?? new List<FormGroupRuleModel>(),
                FieldList = new List<FieldListModel>(),
            };

            logWriter?.LogInfo(
                $"FormGroupEventPayloadReader: addformgroup parsed Rules count={model.Rules?.Count ?? 0}",
                "FormGroupEventPayloadReader",
                "ReadForAdd");

            return model;
        }

        private static string GetStringProperty(JObject root, string propertyName)
        {
            var token = root.Properties()
                .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));
            return token?.Value?.Type == JTokenType.Null ? null : token?.Value?.ToString();
        }

        private static int CountArrayItems(string json, string propertyName)
        {
            try
            {
                var root = JObject.Parse(json);
                var array = root[propertyName] ?? root.Properties()
                    .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase))?.Value;
                return array is JArray jArray ? jArray.Count : 0;
            }
            catch (JsonReaderException)
            {
                return -1;
            }
        }
    }
}
