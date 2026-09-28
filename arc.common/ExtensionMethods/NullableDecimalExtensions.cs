using System;
using System.Globalization;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Parses optional decimal values from form payloads and JSON tokens.
/// </summary>
public static class NullableDecimalExtensions
{
    /// <summary>
    /// Parses an optional measurement value from a form field. Empty or whitespace strings map to null;
    /// explicit zero is preserved.
    /// </summary>
    /// <param name="value">Raw form value (string, numeric, or null).</param>
    /// <returns>A nullable decimal, or null when the value is unset.</returns>
    public static decimal? ParseOptionalMeasurementValue(object value)
    {
        if (value == null)
        {
            return null;
        }

        if (value is string s)
        {
            if (string.IsNullOrWhiteSpace(s) || s.Equals("null", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : null;
        }

        return value switch
        {
            decimal d => d,
            double dbl => (decimal)dbl,
            float f => (decimal)f,
            int i => i,
            long l => l,
            _ => null
        };
    }
}
