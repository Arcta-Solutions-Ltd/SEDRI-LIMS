using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Utils;

/// <summary>
/// Provides functionality for removing JSON elements based on their values.
/// </summary>
public class JsonElementRemover : IJsonElementRemover
{
    /// <summary>
    /// Removes elements from a JSON object by matching their values.
    /// </summary>
    /// <param name="json">The JSON string to process.</param>
    /// <param name="value">The value used to identify elements to remove.</param>
    /// <returns>
    /// A JSON string with the specified elements removed.
    /// </returns>
    public string RemoveElementsByValue(string json, string value)
    {
        var jObject = (JObject)JsonConvert.DeserializeObject(json);

        List<JProperty> jProperties = jObject.Properties().ToList();
        for (int i = 0; i < jProperties.Count; i++)
        {
            JProperty jProperty = jProperties[i];
            if (jProperty.Name.ToLower() == "testresults")
            {
                var elementValue = jProperty.First.ToString();
                var newObject = (JObject)JsonConvert.DeserializeObject(elementValue);
                jObject[jProperty.Name] = newObject;
            }
        }

        RemoveElements(jObject, value);

        return JsonConvert.SerializeObject(jObject);
    }

    /// <summary>
    /// Recursively removes elements from a JSON object by their names or values.
    /// </summary>
    /// <param name="jObject">The JSON object to process.</param>
    /// <param name="value">The value used to identify elements to remove.</param>
    private void RemoveElements(JObject jObject, string value)
    {
        List<JProperty> jProperties = jObject.Properties().ToList();
        for (int i = 0; i < jProperties.Count; i++)
        {
            JProperty jProperty = jProperties[i];
            if (jProperty.Name.ToLower() == value.ToLower())
            {
                jProperty.Remove();
            }
            else
            {
                if (jProperty.Value.Type == JTokenType.Array)
                {
                    RemoveFromArray((JArray)jProperty.Value, value);
                }
                else if (jProperty.Value.Type == JTokenType.Object)
                {
                    RemoveElements((JObject)jProperty.Value, value);
                }
                else
                {
                    var elementValue = jProperty.First.ToString();
                    if (elementValue.Contains(value))
                    {
                        jProperty.Remove();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Processes a JSON array to remove elements by their values.
    /// </summary>
    /// <param name="jArray">The JSON array to process.</param>
    /// <param name="value">The value used to identify elements to remove.</param>
    private void RemoveFromArray(JArray jArray, string value)
    {
        foreach (var jObject in jArray)
        {
            if (jObject.Type == JTokenType.Object)
            {
                RemoveElements((JObject)jObject, value);
            }
        }
    }
}
