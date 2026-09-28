using arc.common.ExtensionMethods;
using arc.common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace arc.app.Utils;

/// <summary>
/// Utility class for handling MoreData JSON operations, including merging dynamic fields
/// from MoreData into the main object structure and removing the MoreData property.
/// </summary>
public static class MoreDataUtils
{
    /// <summary>
    /// Merges MoreData fields into the main object and removes the MoreData property.
    /// </summary>
    /// <param name="jsonString">The JSON string representation of the object containing MoreData.</param>
    /// <param name="logWriter">The log writer for recording errors and debug information.</param>
    /// <returns>A new JSON string with MoreData fields flattened into the main object and MoreData property removed.</returns>
    public static string MergeMoreDataFields(string jsonString)
    {
        if (string.IsNullOrWhiteSpace(jsonString))
            return jsonString;

        var jsonObject = JObject.Parse(jsonString);

        if (jsonObject["MoreData"] != null && jsonObject["MoreData"].Type != JTokenType.Null)
        {
            JObject moreData = null;

            // Check if MoreData is a JSON string that needs to be parsed
            if (jsonObject["MoreData"].Type == JTokenType.String)
            {
                var moreDataString = jsonObject["MoreData"].Value<string>();
                if (!string.IsNullOrWhiteSpace(moreDataString))
                {
                    try
                    {
                        moreData = JObject.Parse(moreDataString);
                    }
                    catch (JsonException)
                    {
                        // If parsing fails, treat as null
                        moreData = null;
                    }
                }
            }
            else if (jsonObject["MoreData"].Type == JTokenType.Object)
            {
                moreData = jsonObject["MoreData"] as JObject;
            }

            if (moreData != null)
            {
                foreach (var property in moreData.Properties())
                {
                    jsonObject[property.Name] = property.Value;
                }
            }
        }

        // Use RemoveElements extension method to remove MoreData
        var resultJson = jsonObject.ToString(Formatting.None);
        return resultJson.RemoveElements("MoreData");
    }

    /// <summary>
    /// Merges MoreData fields into the main object and removes the MoreData property.
    /// This overload works directly with a JObject.
    /// </summary>
    /// <param name="jsonObject">The JObject containing MoreData.</param>
    /// <param name="logWriter">The log writer for recording errors and debug information.</param>
    /// <returns>A new JObject with MoreData fields flattened and MoreData property removed.</returns>
    public static JObject MergeMoreDataFields(JObject jsonObject)
    {
        if (jsonObject == null)
            return jsonObject;

        var result = jsonObject.DeepClone() as JObject;

        if (result["MoreData"] != null && result["MoreData"].Type != JTokenType.Null)
        {
            JObject moreData = null;

            // Check if MoreData is a JSON string that needs to be parsed
            if (result["MoreData"].Type == JTokenType.String)
            {
                var moreDataString = result["MoreData"].Value<string>();
                if (!string.IsNullOrWhiteSpace(moreDataString))
                {
                    try
                    {
                        moreData = JObject.Parse(moreDataString);
                    }
                    catch (JsonException)
                    {
                        // If parsing fails, treat as null
                        moreData = null;
                    }
                }
            }
            else if (result["MoreData"].Type == JTokenType.Object)
            {
                moreData = result["MoreData"] as JObject;
            }

            if (moreData != null)
            {
                foreach (var property in moreData.Properties())
                {
                    result[property.Name] = property.Value;
                }
            }
        }

        // Use RemoveElements extension method to remove MoreData
        var resultJson = result.ToString(Formatting.None);
        return JObject.Parse(resultJson.RemoveElements("MoreData"));

    }
}
