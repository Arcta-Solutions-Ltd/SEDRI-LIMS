using Newtonsoft.Json;

namespace arc.common.Utils
{
    /// <summary>
    /// Contains static methods for Serializing and Deserializing between JSON strings and .NET objects. Ignores missing members and nulls.
    /// </summary>
    public static class ArcJson
    {
        /// <summary>
        /// Deserializes the JSON string to the specified .Net object.
        /// </summary>
        /// <param name="message">JSON string to deserialize</param>
        /// <typeparam name="T">The class to deserialize the JSON string to</typeparam>
        /// <returns>The deserialized object created from the JSON string</returns>
        public static T Deserialize<T>(string message)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(message, settings);
        }

        /// <summary>
        /// Serializes the specified .NET class to a JSON string.
        /// </summary>
        /// <param name="message">.NET object to serialize</param>
        /// <returns>JSON string</returns>
        public static string Serialize(object message)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            return JsonConvert.SerializeObject(message, settings);
        }
    }
}
