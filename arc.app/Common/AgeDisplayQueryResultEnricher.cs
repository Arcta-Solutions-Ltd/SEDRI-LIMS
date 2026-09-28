using arc.common.ExtensionMethods;
using Newtonsoft.Json.Linq;
using System;

namespace arc.app.Common
{
    /// <summary>
    /// Enriches mapped patient and specimen record view JSON with calculated age display values.
    /// </summary>
    public class AgeDisplayQueryResultEnricher : IAgeDisplayQueryResultEnricher
    {
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgeDisplayQueryResultEnricher"/> class.
        /// </summary>
        /// <param name="logWriter">The log writer for production diagnostics.</param>
        public AgeDisplayQueryResultEnricher(ILogWriter logWriter)
        {
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public string EnrichPatientViewAge(string mappedJson, string rawQueryJson)
        {
            var dob = GetFieldValue(rawQueryJson, "DateOfBirth");
            if (string.IsNullOrWhiteSpace(dob))
            {
                _logWriter.LogInfo("Patient view age display skipped: DateOfBirth is empty", "AgeDisplayQueryResultEnricher", "EnrichPatientViewAge");
                return RemoveFields(mappedJson, "ageyears", "agemonths");
            }

            var dobDate = AgeDisplayExtensions.TryParseQueryDate(dob);
            if (!dobDate.HasValue)
            {
                _logWriter.LogInfo($"Patient view age display skipped: invalid DateOfBirth '{dob}'", "AgeDisplayQueryResultEnricher", "EnrichPatientViewAge");
                return RemoveFields(mappedJson, "ageyears", "agemonths");
            }

            var referenceDate = DateTime.UtcNow;
            var age = AgeDisplayExtensions.CalculateAge(dobDate.Value, referenceDate);
            var display = AgeDisplayExtensions.FormatAgeDisplay(age);

            _logWriter.LogInfo(
                $"Patient view age display: DOB={dobDate:yyyy-MM-dd}, Reference={referenceDate:yyyy-MM-dd HH:mm}, Display='{display}'",
                "AgeDisplayQueryResultEnricher",
                "EnrichPatientViewAge");

            return SetFieldValue(RemoveFields(mappedJson, "ageyears", "agemonths"), "patientagedisplay", display);
        }

        /// <inheritdoc />
        public string EnrichSpecimenViewAge(string mappedJson, string rawQueryJson)
        {
            var dob = GetFieldValue(rawQueryJson, "DateOfBirth");
            if (string.IsNullOrWhiteSpace(dob))
            {
                _logWriter.LogInfo("Specimen view age display skipped: patient DateOfBirth is empty", "AgeDisplayQueryResultEnricher", "EnrichSpecimenViewAge");
                return RemoveFields(mappedJson, "ageatspecimen");
            }

            var dobDate = AgeDisplayExtensions.TryParseQueryDate(dob);
            if (!dobDate.HasValue)
            {
                _logWriter.LogInfo($"Specimen view age display skipped: invalid DateOfBirth '{dob}'", "AgeDisplayQueryResultEnricher", "EnrichSpecimenViewAge");
                return RemoveFields(mappedJson, "ageatspecimen");
            }

            var collectionDate = GetFieldValue(rawQueryJson, "CollectionDate");
            var collectionTime = GetFieldValue(rawQueryJson, "CollectionTime");
            var receivedDate = GetFieldValue(rawQueryJson, "ReceivedDate");
            var receivedTime = GetFieldValue(rawQueryJson, "ReceivedTime");

            var referenceDate = AgeDisplayExtensions.CombineDateAndTime(collectionDate, collectionTime)
                ?? AgeDisplayExtensions.CombineDateAndTime(receivedDate, receivedTime)
                ?? DateTime.UtcNow;

            var age = AgeDisplayExtensions.CalculateAge(dobDate.Value, referenceDate);
            var display = AgeDisplayExtensions.FormatAgeDisplay(age);

            _logWriter.LogInfo(
                $"Specimen view age display: DOB={dobDate:yyyy-MM-dd}, Reference={referenceDate:yyyy-MM-dd HH:mm}, Display='{display}'",
                "AgeDisplayQueryResultEnricher",
                "EnrichSpecimenViewAge");

            return SetFieldValue(RemoveFields(mappedJson, "ageatspecimen"), "patientageatspecimen", display);
        }

        private static string GetFieldValue(string rawQueryJson, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(rawQueryJson))
            {
                return null;
            }

            try
            {
                var token = JToken.Parse(rawQueryJson);
                if (token is JObject obj)
                {
                    foreach (var property in obj.Properties())
                    {
                        if (string.Equals(property.Name, fieldName, StringComparison.OrdinalIgnoreCase))
                        {
                            return property.Value?.Type == JTokenType.Null ? null : property.Value.ToString();
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string SetFieldValue(string mappedJson, string fieldId, string value)
        {
            if (string.IsNullOrWhiteSpace(mappedJson))
            {
                return mappedJson;
            }

            try
            {
                var root = JToken.Parse(mappedJson);
                UpdateFieldValues(root, fieldId, value ?? string.Empty);
                return root.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return mappedJson;
            }
        }

        private static string RemoveFields(string mappedJson, params string[] fieldIds)
        {
            if (string.IsNullOrWhiteSpace(mappedJson))
            {
                return mappedJson;
            }

            try
            {
                var root = JToken.Parse(mappedJson);
                foreach (var fieldId in fieldIds)
                {
                    RemoveFieldNodes(root, fieldId);
                }

                return root.ToString(Newtonsoft.Json.Formatting.None);
            }
            catch
            {
                return mappedJson;
            }
        }

        private static void UpdateFieldValues(JToken node, string fieldId, string value)
        {
            if (node == null)
            {
                return;
            }

            if (node.Type == JTokenType.Object)
            {
                var idToken = node["Id"] ?? node["id"];
                if (idToken != null && string.Equals(idToken.ToString(), fieldId, StringComparison.OrdinalIgnoreCase))
                {
                    if (node["Value"] != null)
                    {
                        node["Value"] = value;
                    }
                    else
                    {
                        node["value"] = value;
                    }
                }
            }

            foreach (var child in node.Children())
            {
                UpdateFieldValues(child, fieldId, value);
            }
        }

        private static void RemoveFieldNodes(JToken node, string fieldId)
        {
            if (node == null)
            {
                return;
            }

            if (node.Type == JTokenType.Array)
            {
                var array = (JArray)node;
                for (var i = array.Count - 1; i >= 0; i--)
                {
                    var item = array[i];
                    var idToken = item?["Id"] ?? item?["id"];
                    if (idToken != null && string.Equals(idToken.ToString(), fieldId, StringComparison.OrdinalIgnoreCase))
                    {
                        array.RemoveAt(i);
                    }
                    else
                    {
                        RemoveFieldNodes(item, fieldId);
                    }
                }

                return;
            }

            foreach (var child in node.Children())
            {
                RemoveFieldNodes(child, fieldId);
            }
        }
    }
}
