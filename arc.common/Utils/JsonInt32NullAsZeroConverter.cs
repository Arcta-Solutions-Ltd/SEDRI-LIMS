using Newtonsoft.Json;
using System;
using System.Globalization;

namespace arc.common.Utils;

/// <summary>
/// Deserializes null, empty string, or missing numeric tokens as 0 for AST row fields the portal may send as null (e.g. Disk/MIC dosage before entry).
/// </summary>
public sealed class JsonInt32NullAsZeroConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => objectType == typeof(int);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return 0;
        }

        if (reader.TokenType == JsonToken.Integer)
        {
            return Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
        }

        if (reader.TokenType == JsonToken.Float)
        {
            return Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
        }

        if (reader.TokenType == JsonToken.String && reader.Value is string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return 0;
            }

            return int.Parse(s, CultureInfo.InvariantCulture);
        }

        return 0;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue((int)value);
    }
}
