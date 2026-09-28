using System;
using System.Collections.Generic;
using arc.common.ExtensionMethods;
using arc.common.Models.AST;
using arc.data.model.AST;
using Microsoft.Extensions.Logging;

namespace arc.app.AST;

/// <summary>
/// Merges astsusceptibilityoverride rows onto AST portal rows by stable line keys (ids only).
/// </summary>
public static class AstSusceptibilityOverrideMerger
{
    /// <summary>
    /// Builds a dictionary of override rows keyed by susceptibility line key for a culture.
    /// </summary>
    public static Dictionary<string, AstSusceptibilityOverrideDataModel> BuildLookup(
        IEnumerable<AstSusceptibilityOverrideDataModel> overrides)
    {
        var lookup = new Dictionary<string, AstSusceptibilityOverrideDataModel>();
        if (overrides == null)
        {
            return lookup;
        }

        foreach (var row in overrides)
        {
            var key = AstSusceptibilityOverrideExtensions.BuildSusceptibilityLineKey(
                row.TestMethodId,
                row.AntibioticId,
                row.GuidelinesId,
                row.Dosage,
                row.SpecialConsiderationId);

            if (key.Length > 0)
            {
                lookup[key] = row;
            }
        }

        return lookup;
    }

    /// <summary>
    /// Applies override audit data onto disk and MIC result lists.
    /// </summary>
    public static void MergeOntoAstQuery(
        ASTQueryModel queryModel,
        IReadOnlyList<AstSusceptibilityOverrideDataModel> overrides,
        ILogger logger)
    {
        if (queryModel == null)
        {
            return;
        }

        var lookup = BuildLookup(overrides);
        MergeRows(queryModel.DiskResults, lookup);
        MergeRows(queryModel.MicResults, lookup);

        logger?.LogInformation(
            "AstSusceptibilityOverrideMerger: CultureId={CultureId}, OverrideRowsLoaded={Count}, DiskRows={Disk}, MicRows={Mic}",
            queryModel.CultureId,
            overrides?.Count ?? 0,
            queryModel.DiskResults?.Count ?? 0,
            queryModel.MicResults?.Count ?? 0);
    }

    private static void MergeRows(List<ASTRowModel> rows, Dictionary<string, AstSusceptibilityOverrideDataModel> lookup)
    {
        if (rows == null || lookup.Count == 0)
        {
            return;
        }

        foreach (var row in rows)
        {
            if (row == null || row.ExpertRuleLine)
            {
                continue;
            }

            var parentKey = row.BuildSusceptibilityLineKey(0);
            if (parentKey.Length > 0 && lookup.TryGetValue(parentKey, out var parentOverride))
            {
                row.SusceptibilityOverride = ToDto(parentOverride);
            }

            if (row.EmbeddedASTRows == null)
            {
                continue;
            }

            foreach (var embed in row.EmbeddedASTRows)
            {
                if (embed == null)
                {
                    continue;
                }

                var scId = embed.SpecialConsiderationId;
                var embedKey = row.BuildSusceptibilityLineKey(scId);
                if (embedKey.Length > 0 && lookup.TryGetValue(embedKey, out var embedOverride))
                {
                    embed.SusceptibilityOverride = ToDto(embedOverride);
                }
            }
        }
    }

    /// <summary>
    /// Converts a persisted override row to a portal DTO.
    /// </summary>
    public static AstSusceptibilityOverrideModel ToDto(AstSusceptibilityOverrideDataModel source)
    {
        if (source == null)
        {
            return null;
        }

        return new AstSusceptibilityOverrideModel
        {
            IsManuallySet = source.IsManuallySet == "Yes",
            SetByUsername = source.SetBy ?? "",
            SetAt = source.SetAt,
            CannedCommentId = source.CannedCommentId,
            FreeTextComment = source.FreeTextComment ?? "",
            OverriddenFromSusceptibilityId = source.OverriddenFromSusceptibilityId,
        };
    }

    /// <summary>
    /// Converts a portal DTO to a persisted override row for save.
    /// </summary>
    public static AstSusceptibilityOverrideDataModel ToDataModel(
        int cultureId,
        int testMethodId,
        int antibioticId,
        int guidelinesId,
        int dosage,
        int specialConsiderationId,
        AstSusceptibilityOverrideModel dto,
        string username)
    {
        if (dto == null || !dto.IsManuallySet)
        {
            return null;
        }

        return new AstSusceptibilityOverrideDataModel
        {
            CultureId = cultureId,
            TestMethodId = testMethodId,
            AntibioticId = antibioticId,
            GuidelinesId = guidelinesId,
            Dosage = testMethodId == AstRowModelExtensions.DiskTestMethodId ? dosage : 0,
            SpecialConsiderationId = specialConsiderationId,
            IsManuallySet = "Yes",
            SetBy = string.IsNullOrEmpty(dto.SetByUsername) ? username : dto.SetByUsername,
            SetAt = dto.SetAt ?? DateTime.UtcNow,
            CannedCommentId = dto.CannedCommentId,
            FreeTextComment = dto.FreeTextComment ?? "",
            OverriddenFromSusceptibilityId = dto.OverriddenFromSusceptibilityId,
        };
    }
}
