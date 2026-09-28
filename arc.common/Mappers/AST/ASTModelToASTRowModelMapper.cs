using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using System.Collections.Generic;
using System.Linq;

namespace arc.common.Mappers.AST;

/// <summary>
/// Maps AST database models to AST row models for display. Populates EmbeddedASTRows from
/// SpecialRows (specialastrow) so stored susceptibility and IncludeOnReport values are preserved.
/// The handler must still call SpecialConsiderationEnricher to merge with breakpoint-derived rows
/// so all special consideration types are displayed (including those without stored values).
/// </summary>
public class ASTModelToASTRowModelMapper : IMapWithList<ASTModel, ASTRowModel>
{
    /// <summary>
    /// Maps a stored disk measurement to a nullable zone diameter; blank sentinel (-1) becomes null.
    /// </summary>
    private static int? MapDiskMeasurement(string measurement)
    {
        var numeric = MicMeasurementExtensions.ParseDiskMeasurementNumeric(measurement);
        return numeric <= 0 ? null : (int)numeric;
    }

    /// <summary>
    /// Maps a stored MIC measurement to a nullable decimal; blank sentinel (-1) becomes null.
    /// </summary>
    private static decimal? MapMicMeasurement(string measurement)
    {
        var numeric = MicMeasurementExtensions.ParseMicNumericForPersist(measurement);
        return numeric <= 0 ? null : numeric;
    }

    /// <summary>
    /// Mic comparison prefix from storage, or inferred from the combined Measurement string when MicComparison is empty.
    /// </summary>
    private static string ExtractMicOperator(string micComparison, string measurement)
    {
        if (!string.IsNullOrWhiteSpace(micComparison))
        {
            return micComparison.Trim();
        }

        if (measurement.StartsWith("<=", System.StringComparison.Ordinal))
        {
            return "<=";
        }

        if (measurement.StartsWith(">=", System.StringComparison.Ordinal))
        {
            return ">=";
        }

        if (measurement.StartsWith("<", System.StringComparison.Ordinal))
        {
            return "<";
        }

        if (measurement.StartsWith(">", System.StringComparison.Ordinal))
        {
            return ">";
        }

        return string.Empty;
    }

    /// <summary>
    /// Maps an AST model to an AST row model. Populates EmbeddedASTRows from SpecialRows when present,
    /// with each special row's TestResult (susceptibility) and IncludeOnReport inherited from storage.
    /// </summary>
    public ASTRowModel Map(ASTModel source)
    {
        var result = source.Map<ASTRowModel>();
        // IncludeInReport (storage) vs IncludeOnReport (UI row) — ObjectExtensions.Map does not map across names.
        result.IncludeOnReport = source.IncludeInReport ?? "No";
        result.AppliedBreakpointId = source.AppliedBreakpointId;
        result.Antibiotic = source.AntibioticId;
        result.Guidelines = source.GuidelinesId;
        result.TestMethod = source.TestMethodId;
        result.TestResult = source.SusceptibilityId;
        result.TestMethod = source.TestMethodId;
        result.DrugCategory = source.CategoryId;
        // Disk (681) vs MIC (680) is keyed by TestMethodId in ASTHandler and the UI. DB TestType may not
        // always be "disk"/"strip", so parse Measurement using TestMethodId when it is 681 or 680.
        // No-measurement sentinel (-1) maps to null on Mic/ZoneDiameter for UI and expert-rule evaluation.
        if (source.TestMethodId == 681)
        {
            result.ZoneDiameter = MapDiskMeasurement(source.Measurement);
            result.Mic = null;
        }
        else if (source.TestMethodId == 680)
        {
            result.ZoneDiameter = null;
            result.Mic = MapMicMeasurement(source.Measurement);
            result.Operator = ExtractMicOperator(source.MicComparison, source.Measurement);
        }
        else
        {
            result.ZoneDiameter = string.Equals(source.TestType, "disk", System.StringComparison.OrdinalIgnoreCase)
                ? MapDiskMeasurement(source.Measurement)
                : null;
            if (string.Equals(source.TestType, "strip", System.StringComparison.OrdinalIgnoreCase))
            {
                result.Mic = MapMicMeasurement(source.Measurement);
                result.Operator = ExtractMicOperator(source.MicComparison, source.Measurement);
            }
            else
            {
                result.Mic = null;
            }
        }

        if (source.SpecialRows != null && source.SpecialRows.Any())
        {
            result.EmbeddedASTRows = new List<ASTRowModel>();
            foreach (var row in source.SpecialRows)
            {
                var embeddedRow = new ASTRowModel
                {
                    Antibiotic = source.AntibioticId,
                    Guidelines = source.GuidelinesId,
                    Dosage = source.Dosage,
                    TestMethod = source.TestMethodId,
                    DrugCategory = source.CategoryId,
                    SpecialConsiderationId = row.SpecialTypeId,
                    SpecialConsideration = row.Name,
                    TestResult = row.SusceptibilityId,
                    IncludeOnReport = row.IncludeInReport ?? "No",
                    AppliedBreakpointId = row.BreakpointId
                };
                result.EmbeddedASTRows.Add(embeddedRow);
            }
        }

        return result;
    }

    /// <summary>
    /// Maps a single AST model to a list containing the main row and its embedded special rows.
    /// Prefer Map for AST display; MapList is used when flattening is needed.
    /// </summary>
    public List<ASTRowModel> MapToList(ASTModel source)
    {
        var mapped = Map(source);
        var result = new List<ASTRowModel> { mapped };
        if (mapped.EmbeddedASTRows != null)
        {
            result.AddRange(mapped.EmbeddedASTRows);
        }
        return result;
    }

    /// <summary>
    /// Maps a list of AST models to AST row models. Each item's EmbeddedASTRows is populated from SpecialRows.
    /// </summary>
    public List<ASTRowModel> MapList(List<ASTModel> source)
    {
        return source.Select(s => Map(s)).ToList();
    }
} 

