using System;
using System.Globalization;

namespace arc.common.ExtensionMethods;

/// <summary>
/// Result of parsing an AST MIC measurement string for save and persist.
/// </summary>
public readonly struct MicMeasurementParts
{
    /// <summary>Parsed numeric portion of the measurement, or 0 when absent.</summary>
    public decimal NumericValue { get; init; }

    /// <summary>Comparison operator prefix: empty, &gt;, &lt;=, or legacy &lt; from stored data.</summary>
    public string ComparisonOperator { get; init; }

    /// <summary>True when the measurement is null, empty, or the -1 no-measurement sentinel.</summary>
    public bool IsBlank { get; init; }

    /// <summary>True when a valid decimal numeric portion was parsed.</summary>
    public bool HasNumericValue { get; init; }

    /// <summary>True when only an operator prefix is present with no numeric portion (e.g. &gt;, &lt;=, legacy &lt;).</summary>
    public bool IsOperatorOnly { get; init; }
}

/// <summary>
/// Parses AST MIC measurement strings (optional comparison prefix plus numeric value).
/// </summary>
public static class MicMeasurementExtensions
{
    /// <summary>
    /// Parses a MIC measurement string into operator and numeric components.
    /// </summary>
    /// <param name="measurement">Raw measurement from the craft payload or database round-trip.</param>
    /// <param name="parts">Parsed result.</param>
    /// <returns>Always returns true; inspect <see cref="MicMeasurementParts"/> flags for validity.</returns>
    public static bool TryParseMicMeasurement(string? measurement, out MicMeasurementParts parts)
    {
        parts = default;

        if (IsBlankMeasurement(measurement))
        {
            parts = new MicMeasurementParts { IsBlank = true, ComparisonOperator = string.Empty };
            return true;
        }

        var trimmed = measurement!.Trim();
        var op = string.Empty;
        var numStr = trimmed;

        if (trimmed.StartsWith("<=", StringComparison.Ordinal))
        {
            op = "<=";
            numStr = trimmed.Substring(2).Trim();
        }
        else if (trimmed.StartsWith("<", StringComparison.Ordinal))
        {
            op = "<";
            numStr = trimmed.Substring(1).Trim();
        }
        else if (trimmed.StartsWith(">=", StringComparison.Ordinal))
        {
            op = ">=";
            numStr = trimmed.Substring(2).Trim();
        }
        else if (trimmed.StartsWith(">", StringComparison.Ordinal))
        {
            op = ">";
            numStr = trimmed.Substring(1).Trim();
        }

        if (string.IsNullOrEmpty(numStr))
        {
            parts = new MicMeasurementParts
            {
                ComparisonOperator = op,
                IsOperatorOnly = !string.IsNullOrEmpty(op),
                HasNumericValue = false
            };
            return true;
        }

        if (!decimal.TryParse(numStr, NumberStyles.Number, CultureInfo.InvariantCulture, out var numeric))
        {
            parts = new MicMeasurementParts
            {
                ComparisonOperator = op,
                HasNumericValue = false,
                IsOperatorOnly = false
            };
            return true;
        }

        parts = new MicMeasurementParts
        {
            NumericValue = numeric,
            ComparisonOperator = op,
            HasNumericValue = true,
            IsOperatorOnly = false
        };
        return true;
    }

    /// <summary>
    /// Returns the numeric portion for persist, or -1 when the measurement is blank.
    /// </summary>
    /// <param name="measurement">Raw measurement string.</param>
    /// <returns>Parsed numeric value, or -1 for blank/no measurement.</returns>
    public static decimal ParseMicNumericForPersist(string? measurement)
    {
        TryParseMicMeasurement(measurement, out var parts);
        if (parts.IsBlank)
        {
            return -1m;
        }

        if (parts.HasNumericValue)
        {
            return parts.NumericValue;
        }

        return -1m;
    }

    /// <summary>
    /// Returns the numeric portion for disk zone diameter validation, or -1 when blank or unparseable.
    /// </summary>
    /// <param name="measurement">Raw measurement string.</param>
    /// <returns>Parsed numeric value, or -1 when no measurement is present.</returns>
    public static decimal ParseDiskMeasurementNumeric(string? measurement)
    {
        TryParseMicMeasurement(measurement, out var parts);
        if (parts.IsBlank)
        {
            return -1m;
        }

        if (parts.HasNumericValue)
        {
            return parts.NumericValue;
        }

        return -1m;
    }

    /// <summary>
    /// Returns the MIC comparison operator prefix for database storage.
    /// </summary>
    /// <param name="measurement">Raw measurement string.</param>
    /// <returns>Operator prefix or empty string when none.</returns>
    public static string GetMicComparisonOperator(string? measurement)
    {
        TryParseMicMeasurement(measurement, out var parts);
        return parts.ComparisonOperator ?? string.Empty;
    }

    /// <summary>
    /// Returns the numeric portion for breakpoint lookup (0 when blank).
    /// </summary>
    /// <param name="measurement">Raw measurement string.</param>
    /// <returns>Parsed numeric value, or 0 when blank.</returns>
    public static decimal ParseMeasurementNumericForLookup(string? measurement)
    {
        TryParseMicMeasurement(measurement, out var parts);
        if (parts.IsBlank || !parts.HasNumericValue)
        {
            return 0m;
        }

        return parts.NumericValue;
    }

    private static bool IsBlankMeasurement(string? measurement)
    {
        if (string.IsNullOrWhiteSpace(measurement))
        {
            return true;
        }

        if (measurement == "-1")
        {
            return true;
        }

        if (decimal.TryParse(measurement, NumberStyles.Number, CultureInfo.InvariantCulture, out var numeric) && numeric == -1m)
        {
            return true;
        }

        return false;
    }
}
